using System.Diagnostics;


namespace TallyApp
{
    public partial class MainPage : ContentPage
    {
        private string currentInput = "";
        private double total = 0;

        public MainPage()
        {
            InitializeComponent();
            SetupLayout(); // Sets up layout after Initialize Component
            SizeChanged += OnPageSizeChanged; // Subscribing to screen size changes
        }

        // Orientation detection for layout changes
        private void SetupLayout()
        {
            bool isPortrait = Height > Width;
            if (isPortrait)
            {
                SetupPortraitLayout();
            }
            else
            {
                SetupLandscapeLayout();
            }
        }

        // For the number button click: The button number becomes the currentInput and adds this accordingly
        private void OnNumberClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;

            SetupLayout();
            // Check if currentInput is empty to start a new number
            if (currentInput == "")
            {
                currentInput = button.Text;
                tallyDisplay.Text += "\n" + currentInput; // Start on a new line
            }
            else
            {
                currentInput += button.Text;
                // Update the last line of tallyDisplay with the new currentInput
                var lines = tallyDisplay.Text.Split('\n');
                lines[^1] = currentInput; // ^1 is the index of the last element
                tallyDisplay.Text = string.Join("\n", lines);
            }
        }

        // Adds the number to the total
        private void OnPlusClicked(object sender, EventArgs e)
        {
            SetupLayout();

            if (double.TryParse(currentInput, out double number))
            {
                total += number;
                tallyDisplay.Text += " + ";
                totalLabel.Text = "Total: " + total.ToString();
            }
            currentInput = "";
        }

        // Clears and resets values
        private void OnClearClicked(object sender, EventArgs e)
        {
            currentInput = "";
            total = 0;
            tallyDisplay.Text = "";
            totalLabel.Text = "Total: 0";
        }

        // Subscribing to event when the page size is changes. Adjusts layout accordingly 
        [Obsolete]
        private void OnPageSizeChanged(object sender, EventArgs e)
        {
            Debug.WriteLine($"Size changed. Width: {this.Width}, Height: {this.Height}");

            Device.BeginInvokeOnMainThread(() =>
            {
                bool isPortrait = Height > Width;
                if (isPortrait)
                {
                    SetupPortraitLayout();
                }
                else
                {
                    SetupLandscapeLayout();
                }
            });
        }


        private void SetupLandscapeLayout()
        {
            // Define columns for landscape orientation
            DynamicLayout.ColumnDefinitions.Clear();
            DynamicLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // Flexible width
            DynamicLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); 
            DynamicLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); 

            // No row definitions needed for landscape
            DynamicLayout.RowDefinitions.Clear();

            // Set the elements to the respective columns
            Grid.SetColumn(buttonsGrid, 0);
            Grid.SetColumn(tallyDisplayFrame, 1); 
            Grid.SetColumn(totalLabel, 2);

            Grid.SetRow(buttonsGrid, 0);
            Grid.SetRow(tallyDisplayFrame, 0);
            Grid.SetRow(totalLabel, 0);

            // Add Margins for spacing
            buttonsGrid.Margin = new Thickness(10, 0, 10, 0); 
            tallyDisplayFrame.Margin = new Thickness(10, 0, 10, 0); 
            totalLabel.HorizontalOptions = LayoutOptions.Center;
            totalLabel.VerticalOptions = LayoutOptions.Center;
            totalLabel.Margin = new Thickness(10, 0, 10, 0);
        }

        private void SetupPortraitLayout()
        {
            // Define rows for portrait orientation
            DynamicLayout.RowDefinitions.Clear();
            DynamicLayout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            DynamicLayout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            DynamicLayout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            // Clear column definitions for portrait
            DynamicLayout.ColumnDefinitions.Clear();

            // Set the elements to the respective rows
            Grid.SetRow(tallyDisplayFrame, 0);
            Grid.SetRow(totalLabel, 1);
            Grid.SetRow(buttonsGrid, 2);

            Grid.SetColumn(tallyDisplayFrame, 0);
            Grid.SetColumn(totalLabel, 0);
            Grid.SetColumn(buttonsGrid, 0);

            // Reset HorizontalOptions for totalLabel in portrait mode
            totalLabel.HorizontalOptions = LayoutOptions.End;
            totalLabel.Margin = new Thickness(0, 0, 60, 0);
        }

        // Sets up the layout on when the screen appears
        protected override void OnAppearing()
        {
            base.OnAppearing();
            SetupLayout();
        }
    }
}