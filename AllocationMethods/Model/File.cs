using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AllocationMethods.Model
{
    public class File:ViewModelBase
    {
        #region Fields

        private string _name;
        private string _textName;
        private int _blockLength;
        private int _size;
        private Color _fileColor;
        #endregion

        #region Constructors

        /// <summary>
        /// Option 1: File with name and block length
        /// </summary>
        public File(string name, int blockLength) 
        {
            Name = name;
            BlockLength = blockLength;
        }

        ///// <summary>
        ///// Option 2: File with name and size
        ///// </summary>
        //public File(int name, int size) 
        //{
        //    Name = name;
        //    Size = size;
        //}

        #endregion

        #region Properties

        /// <summary>
        /// Random filename to represent the file stored within the block
        /// </summary>
        public string Name
        {
            get { return _name; }
            set 
            { 
                _name = value;
                FileColor = (Color)ColorConverter.ConvertFromString(_name);
            }
        }

        /// <summary>
        /// Option 1: Number of blocks that the file will occupy
        /// </summary>
        public int BlockLength
        {
            get { return _blockLength; }
            set { _blockLength = value; }
        }

        /// <summary>
        /// Option 2: Size of file, to be used to determine how much space it takes up.
        /// </summary>
        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }

        /// <summary>
        /// Take advantage of string name, if necessary
        /// </summary>
        public string TextName
        {
            get { return _textName; }
            set { _textName = value; }
        }

        public Color FileColor
        {
            get { return _fileColor; }
            set { _fileColor = value;}
        }


        #endregion
    }
}
