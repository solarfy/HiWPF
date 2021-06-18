using PlayJeeDeTacquin;     //添加项目引用 Panels -> PlayJeeDeTacquin
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace JeuDeTacquin
{
    /// <summary>
    /// JeuDeTacquinPage.xaml 的交互逻辑
    /// </summary>
    public partial class JeuDeTacquinPage : Page
    {
        public int NumberRows = 4;
        public int NumberCols = 4;
        bool isLoaded = false;  /* Back和Forward也会触发Loaded事件，通过isLoaded标记防止同一个Page加载多次 */

        int xEmpty, yEmpty, iCounter;
        Key[] keys = { Key.Left, Key.Right, Key.Up, Key.Down };
        Random rand;
        UIElement elEmptySpare = new Empty();

        public JeuDeTacquinPage()
        {
            this.Loaded += PageOnLoaded;
            InitializeComponent();
        }

        void PageOnLoaded(object sender, RoutedEventArgs args)
        {
            if (!isLoaded)
            {
                this.Title = string.Format("Jeu de Tacquin ({0}\x00D7{1})", NumberCols, NumberRows);
                this.unigrid.Rows = NumberRows;
                this.unigrid.Columns = NumberCols;

                for (int i = 0; i < NumberRows * NumberCols - 1; i++)
                {
                    Tile tile = new Tile();
                    tile.Text = (i + 1).ToString();
                    tile.MouseLeftButtonDown += TileOnMouseLeftButtonDown;
                    unigrid.Children.Add(tile);
                }

                unigrid.Children.Add(new Empty());
                xEmpty = NumberCols - 1;
                yEmpty = NumberRows - 1;

                isLoaded = true;
            }

            Focus();
        }

        private void TileOnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Focus();

            Tile tile = sender as Tile;

            int iMove = unigrid.Children.IndexOf(tile);
            int xMove = iMove % NumberCols;
            int yMove = iMove / NumberCols;

            if (xMove == xEmpty)
            {
                while (yMove != yEmpty)
                    MoveTile(xMove, yEmpty + (yMove - yEmpty) / Math.Abs(yMove - yEmpty));            
            }

            if (yMove == yEmpty)
            {
                while (xMove != xEmpty)
                    MoveTile(xEmpty + (xMove - xEmpty) / Math.Abs(xMove - xEmpty), yMove);
            }

            e.Handled = true;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.Key)
            {
                case Key.Right: MoveTile(xEmpty - 1, yEmpty);break;
                case Key.Left: MoveTile(xEmpty + 1, yEmpty);break;
                case Key.Down: MoveTile(xEmpty, yEmpty - 1);break;
                case Key.Up: MoveTile(xEmpty, yEmpty + 1);break;
                default:return;
            }
            e.Handled = true;
        }

        private void ScrambleOnClick(object sender, RoutedEventArgs e)
        {
            rand = new Random();

            iCounter = 16 * NumberCols * NumberRows;

            DispatcherTimer tmr = new DispatcherTimer();
            tmr.Interval = TimeSpan.FromMilliseconds(10);
            tmr.Tick += TimerOnTick;
            tmr.Start();
        }

        private void TimerOnTick(object sender, EventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                MoveTile(xEmpty, yEmpty + rand.Next(3) - 1);
                MoveTile(xEmpty + rand.Next(3) - 1, yEmpty);
            }

            if (0 == iCounter--)
                (sender as DispatcherTimer).Stop();
        }

        private void NextOnClick(object sender, RoutedEventArgs e)
        {
            if (!NavigationService.CanGoForward)
            {
                JeuDeTacquinPage page = new JeuDeTacquinPage();
                page.NumberRows = NumberRows + 1;
                page.NumberCols = NumberCols + 1;

                NavigationService.Navigate(page);
            }
            else
                NavigationService.GoForward();
        }

        void MoveTile(int xTile, int yTile)
        {
            if ((xTile == xEmpty && yTile == yEmpty) || xTile < 0 || xTile >= NumberCols || yTile < 0 || yTile >= NumberCols)
                return;

            int iTile = NumberCols * yTile + xTile;
            int iEmpty = NumberCols * yEmpty + xEmpty;

            UIElement elTile = unigrid.Children[iTile];
            UIElement elEmpty = unigrid.Children[iEmpty];

            unigrid.Children.RemoveAt(iTile);
            unigrid.Children.Insert(iTile, elEmptySpare);
            unigrid.Children.RemoveAt(iEmpty);
            unigrid.Children.Insert(iEmpty, elTile);

            xEmpty = xTile;
            yEmpty = yTile;
            elEmptySpare = elEmpty;
        }
    }
}
