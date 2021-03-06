﻿namespace Gu.Wpf.ModernUI.Demo.Content
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for ControlsStylesItemsControl.xaml
    /// </summary>
    public partial class ControlsStylesItemsControl : UserControl
    {
        public ControlsStylesItemsControl()
        {
            this.InitializeComponent();
        }

        private MenuItem CreateSubMenu(string header)
        {
            var item = new MenuItem { Header = header };
            item.Items.Add(new MenuItem { Header = "Item 1" });
            item.Items.Add("Item 2");
            item.Items.Add(new Separator());
            item.Items.Add("Item 3");
            return item;
        }

        private void ShowContextMenu_Click(object sender, RoutedEventArgs e)
        {
            var contextMenu = new ContextMenu();

            contextMenu.Items.Add(new MenuItem { Header = "Item" });
            contextMenu.Items.Add(new MenuItem { Header = "Item with gesture", InputGestureText = "Ctrl+C" });
            contextMenu.Items.Add(new MenuItem { Header = "Item, disabled", IsEnabled = false });
            contextMenu.Items.Add(new MenuItem { Header = "Item, checked", IsChecked = true });
            contextMenu.Items.Add(new MenuItem { Header = "Item, checked and disabled", IsChecked = true, IsEnabled = false });
            contextMenu.Items.Add(new Separator());
            contextMenu.Items.Add(this.CreateSubMenu("Item with submenu"));

            var menu = this.CreateSubMenu("Item with submenu, disabled");
            menu.SetCurrentValue(IsEnabledProperty, false);
            contextMenu.Items.Add(menu);

            contextMenu.IsOpen = true;
        }
    }
}
