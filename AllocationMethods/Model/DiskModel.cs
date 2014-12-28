using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllocationMethods.Model
{
    public class DiskModel
    {
        #region Fields
        private List<FileBlock> _disk = new List<FileBlock>();
        #endregion

        #region Constructors
        #endregion

        #region Properties
        public List<FileBlock> Disk
        {
            get { return _disk; }
            set { _disk = value; }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Fills Disk with file blocks depending on allocation type
        /// </summary>
        private void FillDisk()
        {
            switch(Properties.Settings.Default.AllocationType)
            {
                case AllocationType.Contiguous:
                    FillDiskWithContiguousFileBlocks();
                    break;
                case AllocationType.Indexed:
                    FillDiskWithIndexedFileBlocks();
                    break;
                case AllocationType.Linked:
                    FillDiskWithLinkedFillBlocks();
                    break;
                default:
                    Console.WriteLine("Issue: Allocation Type not being set");
                    break;
            }

            //FIXME : Needs to call event when complete to get disk to update.
        }

        private void FillDiskWithContiguousFileBlocks() 
        {
            for (int i = 0; i < Properties.Settings.Default.DiskBlockCount; i++)
            {
                Disk.Add(new ContiguousFileBlock(i));
            }
        }

        private void FillDiskWithIndexedFileBlocks() 
        {
            for (int i = 0; i < Properties.Settings.Default.DiskBlockCount; i++)
            {
                Disk.Add(new IndexedFileBlock(i));
            }
        }

        private void FillDiskWithLinkedFillBlocks() 
        {
            for (int i = 0; i < Properties.Settings.Default.DiskBlockCount; i++)
            {
                Disk.Add(new LinkedFileBlock(i));
            }
        }

        public void AttemptToDelete(DirectoryEntry entry)
        {
            switch (Properties.Settings.Default.AllocationType)
            {
                case AllocationType.Contiguous:
                    ContiguousFileDirectoryEntry contiguousEntry = (ContiguousFileDirectoryEntry)entry;
                    File contiguousFile = Disk[contiguousEntry.StartingFileBlockAddress].StoredFile;
                    if (entry.FileName != null) 
                    {
                        if (entry.FileName == contiguousFile.Name)
                        {
                            for (int i = contiguousEntry.StartingFileBlockAddress; i < contiguousEntry.StartingFileBlockAddress + contiguousEntry.FileBlockLength; i++)
                            {
                                Disk[i] = new ContiguousFileBlock(i);
                            }
                            //Delete Success
                            OnDeleteSuccess(contiguousFile, contiguousEntry);
                        }
                        else
                        {
                            //Delete Failure
                            OnDeleteFailure(contiguousFile);
                        }
                    }
                    break;
                case AllocationType.Indexed:
                    IndexedFileDirectoryEntry indexedEntry = (IndexedFileDirectoryEntry)entry;
                    IndexedFileBlock indexBlock = (IndexedFileBlock)Disk[indexedEntry.IndexBlockReference];
                    File indexedFile = indexBlock.StoredFile;
                    if (entry.FileName != null) 
                    {
                        if (entry.FileName == indexedFile.Name)
                        {
                            foreach (int address in indexBlock.IndexBlockList)
                            {
                                Disk[address] = new IndexedFileBlock(address);
                            }
                            Disk[indexedEntry.IndexBlockReference] = new IndexedFileBlock(indexedEntry.IndexBlockReference);
                            //Delete Success
                            OnDeleteSuccess(indexedFile, indexedEntry);
                        }
                        else
                        {
                            //Delete Failure
                            OnDeleteFailure(indexedFile);
                        }
                    }
                    break;
                case AllocationType.Linked:
                    LinkedFileDirectoryEntry linkedEntry = (LinkedFileDirectoryEntry) entry;
                    int blockAddressHolder = linkedEntry.StartingFileBlockAddress;
                    LinkedFileBlock tempLinkedFileBlock = (LinkedFileBlock)Disk[blockAddressHolder];
                    File linkedFile = tempLinkedFileBlock.StoredFile;
                    bool endBlockHit = false;
                    if (entry.FileName != null)
                    {
                        if (entry.FileName == linkedFile.Name)
                        {

                            while (!endBlockHit)
                            {
                                Disk[blockAddressHolder] = new LinkedFileBlock(blockAddressHolder);         //Reset Block
                                if (blockAddressHolder != linkedEntry.EndingFileBlockAddress) //Go to next block address only if it is not the end of the file
                                {
                                    blockAddressHolder = tempLinkedFileBlock.NextBlockOfFile;               //Set BlockAddressHolder to nextblock
                                    tempLinkedFileBlock = (LinkedFileBlock)Disk[blockAddressHolder];        //Set tempLinkedFileBlock to nextblock address
                                }
                                else
                                    endBlockHit = true;
                            }
                            //Delete Success
                            OnDeleteSuccess(linkedFile, linkedEntry);
                        }
                        else
                        {
                            //Delete Failure
                            OnDeleteFailure(linkedFile);
                        }
                    }
                    
                    break;
            }

            //Update Disk
            OnDiskChanged();
        }

        /// <summary>
        /// Disk attempts to store file
        /// </summary>
        /// <param name="file"></param>
        public void AttemptToStore(File file) 
        {


            switch (Properties.Settings.Default.AllocationType)
            {
                case AllocationType.Contiguous:

                    //Initialize local utility vars
                    bool emptyLocationFound = false;
                    int numberOfFreeContiguousBlocks = 0;
                    int indexOfFirstFreeContiguousBlock = 0;

                    //Single Loop to reduce overhead
                    for(int i = 0; i < Disk.Count - 1; i++)
                    {
                        if (!Disk[i].IsOccupied && !emptyLocationFound)
                        {
                            if (numberOfFreeContiguousBlocks == 0)                  //Check if this is first open block
                                indexOfFirstFreeContiguousBlock = i;
                            numberOfFreeContiguousBlocks++;                         //Tick up Contiguous Block Count
                            if (numberOfFreeContiguousBlocks >= file.BlockLength)
                            {
                                emptyLocationFound = true;                          //Break loop when location is found long enough
                                break;
                            }
                        }
                        else
                        {
                            numberOfFreeContiguousBlocks = 0;                      //Reset Contiguous Blocks if occupied block present
                        }
                    }
                    if (emptyLocationFound)
                    {
                        //Store File
                        // Create the file blocks for each file
                        for (int y = indexOfFirstFreeContiguousBlock; y < (indexOfFirstFreeContiguousBlock + file.BlockLength); y++)
                        {
                            Disk[y].IsOccupied = true;
                            Disk[y].StoredFile = file;
                        }
                        //Storage Success
                        OnStorageSuccess(new ContiguousFileDirectoryEntry(file.Name.ToString(), indexOfFirstFreeContiguousBlock, file.BlockLength));
                    }
                    else 
                    {
                        //Storage Fail
                        OnStorageFailure(file);
                    }

                    break;

                case AllocationType.Linked:

                    int emptyBlockCount = 0;
                    int indexOfStartingBlock = 0;
                    int indexOfPreviousLinkedBlock = 0;
                    int indexOfEndingBlock = 0;

                    foreach(LinkedFileBlock block in Disk)
                    {
                        if (!block.IsOccupied)
                        {
                            emptyBlockCount++;
                        }
                    }
                    if (emptyBlockCount >= file.BlockLength)
                    {
                        int blockLengthCounter = file.BlockLength;
                        // Scan the disk block by block
                        for (int i = 0; i < Disk.Count - 1; i++)
                        {
                            if (blockLengthCounter != 0) 
                            {
                                if (!Disk[i].IsOccupied)
                                {
                                    
                                    LinkedFileBlock block = new LinkedFileBlock(i, file);

                                    //First Block Condition
                                    if (blockLengthCounter == file.BlockLength)
                                    {
                                        indexOfStartingBlock = i;
                                        indexOfPreviousLinkedBlock = i;
                                    }
                                    else //LinkedFromPreviousBlock 
                                    {
                                        LinkedFileBlock prevBlock = new LinkedFileBlock(indexOfPreviousLinkedBlock, file);
                                        prevBlock.HasPointer = true;
                                        prevBlock.NextBlockOfFile = i;
                                        //Set Past Block as having pointer and proper index
                                        Disk[indexOfPreviousLinkedBlock] = prevBlock;
                                        //Set this current block as new index
                                        indexOfPreviousLinkedBlock = i;
                                    }
                                    //LastBlockCondition
                                    if (blockLengthCounter == 1)
                                    {
                                        indexOfEndingBlock = i;
                                        block.HasPointer = false;
                                        Disk[i] = block;
                                    }
                                    
                                    
                                    //Set current to block
                                    Disk[i] = block;
                                    //Decrease Counter
                                    blockLengthCounter--;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }

                        //Storage Success
                        OnStorageSuccess(new LinkedFileDirectoryEntry(file.Name.ToString(), indexOfStartingBlock, indexOfEndingBlock));
                    }
                    else
                    {
                        //Storage Fails
                        OnStorageFailure(file);
                    }
                    break;

                case AllocationType.Indexed:

                    //Initial Settings
                    int emptyFileBlockCount = 0;
                    int indexedFileBlockLength = file.BlockLength + 1;

                    // Check to see if the disk is large enough to store the file
                    foreach (IndexedFileBlock block in Disk)
                    {
                        if (!block.IsOccupied)
                        {
                            emptyFileBlockCount++;
                        }
                    }

                    
                    if (emptyFileBlockCount >= indexedFileBlockLength)
                    {
                        int indexOfIndexBlock = 0;
                        IndexedFileBlock indexBlock = new IndexedFileBlock(indexOfIndexBlock, true, true, file);

                        for (int i = 0; i < Disk.Count - 1; i++) 
                        {
                            if (indexedFileBlockLength > 0) 
                            {
                                if (!Disk[i].IsOccupied)                    //Look for empty spots
                                {
                                    if (indexedFileBlockLength == file.BlockLength + 1)
                                    {
                                        indexOfIndexBlock = i;
                                    }
                                    else
                                    {
                                        Disk[i] = new IndexedFileBlock(i, true, false, file);
                                        // Add block address to the index block
                                        indexBlock.IndexBlockList.Add(i);
                                    }
                                    indexedFileBlockLength--;
                                }
                                
                            }    
                            else 
                            {    
                                break;
                            }
                            
                        }
                        //Reset Index of Index Block
                        indexBlock.BlockAddress = indexOfIndexBlock;
                        Disk[indexOfIndexBlock] = indexBlock;
                        //Storage Success
                        OnStorageSuccess(new IndexedFileDirectoryEntry(file.Name.ToString(), indexOfIndexBlock));
                    }
                    else 
                    {
                        //Storage Fails
                        OnStorageFailure(file);
                    }
                    break;

                default:
                    Console.WriteLine("Issue: Allocation Type not being set");
                    break;
            }

            //Update Disk
            OnDiskChanged();
                
        }



        /// <summary>
        /// Resets Disk Back to a fresh slate
        /// </summary>
        public void ResetList()
        {
            if(Disk.Count > 0)
                Disk.Clear();
            FillDisk();
            OnDiskChanged();
        }


        #endregion

        #region Events
        public event EventHandler<DiskModelEventArgs> DiskChanged;
        public event EventHandler<DiskModelEventArgs> StorageSuccess;
        public event EventHandler<DiskModelEventArgs> StorageFailure;
        public event EventHandler<DiskModelEventArgs> DeleteSuccess;
        public event EventHandler<DiskModelEventArgs> DeleteFailure;

        public void OnDiskChanged()
        {
            if (DiskChanged != null)
            {
                DiskChanged(this, new DiskModelEventArgs());
            }
        }

        public void OnStorageSuccess(DirectoryEntry directoryEntry)
        {
            if (StorageSuccess != null)
            {
                StorageSuccess(this, new DiskModelEventArgs(directoryEntry));
            }
        }

        public void OnStorageFailure(File file)
        {
            if (StorageFailure != null)
            {
                StorageFailure(this, new DiskModelEventArgs(file));
            }
        }

        public void OnDeleteFailure(File file)
        {
            if (DeleteFailure != null)
            {
                DeleteFailure(this, new DiskModelEventArgs());
            }
        }

        public void OnDeleteSuccess(File file, DirectoryEntry entry)
        {
            if (DeleteSuccess != null)
            {
                DeleteSuccess(this, new DiskModelEventArgs(file, entry));
            }
        }

        #endregion
        
             
    }
}
