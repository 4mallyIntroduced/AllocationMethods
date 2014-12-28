using AllocationMethods.Model;
using GalaSoft.MvvmLight;
using System;
using System.Windows;
using System.Windows.Controls;

namespace AllocationMethods.Services
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class BlockDataTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// Initializes a new instance of the BlockDataTemplateSelector class.
        /// </summary>
        public BlockDataTemplateSelector()
        {

        }

        public DataTemplate FileBlockTemplate { get; set; }
        public DataTemplate ContiguousFileBlockTemplate { get; set; }
        public DataTemplate IndexedFileBlockTemplate { get; set; }
        public DataTemplate LinkedFileBlockTemplate { get; set;}

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var fileblock = item as FileBlock;
            switch (fileblock.Type)
            {
                case AllocationType.Contiguous:
                    return ContiguousFileBlockTemplate;
                case AllocationType.Linked:
                    return LinkedFileBlockTemplate;
                case AllocationType.Indexed:
                    return IndexedFileBlockTemplate;
                default:
                    Console.WriteLine("Issue: Default FileBlockTemplate Used");
                    return FileBlockTemplate;
            }
        }
    }
}