using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RP_Example
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // 後始末
            Closing += (s, e) => { if(DataContext is IDisposable vm) vm.Dispose(); };

            // 追加時スクロール ただしスクロール位置が最下段でなければなにもしない
            if(FindName("ListBox") is ListBox listBox)
            {
                if(listBox.ItemsSource is INotifyCollectionChanged nc)
                {
                    nc.CollectionChanged += (s, e) =>
                    {
                        if(!IsListBoxScrollEnd(listBox)) return;

                        listBox.Items.MoveCurrentToLast();
                        listBox.ScrollIntoView(listBox.Items.CurrentItem);
                    };
                }
            }

            if(FindName("ListView") is ListView listView)
            {
                if(listView.ItemsSource is INotifyCollectionChanged nc)
                {
                    nc.CollectionChanged += (s, e) =>
                    {
                        if(!IsListViewScrollEnd(listView)) return;

                        listView.Items.MoveCurrentToLast();
                        listView.ScrollIntoView(listView.Items.CurrentItem);
                    };
                }
            }
        }

        // http://blog.okazuki.jp/entry/2016/04/12/010634
        private bool IsListBoxScrollEnd(ListBox listBox)
        {
            // 呼ばれた時点ではアイテムは追加されているがコンテナはないので
            // ひとつ前のアイテムが表示されているかで判定
            var index = listBox.Items.Count - 2;
            var container = listBox.ItemContainerGenerator.ContainerFromIndex(index) as ListBoxItem;
            if(container == null)
                return false;

            var box = VisualTreeHelper.GetDescendantBounds(listBox);
            var top = container.TranslatePoint(new Point(), listBox);
            return box.Contains(top);
        }
        // ListViewは動作が怪しいが未調査
        private bool IsListViewScrollEnd(ListView listView)
        {
            var index = listView.Items.Count - 2;
            var container = listView.ItemContainerGenerator.ContainerFromIndex(index) as ListViewItem;
            if(container == null)
                return false;

            var box = VisualTreeHelper.GetDescendantBounds(listView);
            var top = container.TranslatePoint(new Point(), listView);
            return box.Contains(top);
        }
    }
}
