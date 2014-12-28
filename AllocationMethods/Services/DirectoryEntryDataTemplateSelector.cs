using AllocationMethods.Model;
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
    public class DirectoryEntryDataTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// Initializes a new instance of the BlockDataTemplateSelector class.
        /// </summary>
        public DirectoryEntryDataTemplateSelector()
        {

        }

        public DataTemplate DirectoryEntryTemplate { get; set; }
        public DataTemplate ContiguousDirectoryEntryTemplate { get; set; }
        public DataTemplate IndexedDirectoryEntryTemplate { get; set; }
        public DataTemplate LinkedDirectoryEntryTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var directoryEntry = item as DirectoryEntry;
            switch (directoryEntry.Type)
            {
                case AllocationType.Contiguous:
                    return ContiguousDirectoryEntryTemplate;
                case AllocationType.Linked:
                    return LinkedDirectoryEntryTemplate;
                case AllocationType.Indexed:
                    return IndexedDirectoryEntryTemplate;
                default:
                    Console.WriteLine("Issue: Default DirectoryEntryTemplate Used");
                    return DirectoryEntryTemplate;
            }
        }
    }
}