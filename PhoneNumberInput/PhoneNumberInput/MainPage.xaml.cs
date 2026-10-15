using Syncfusion.Maui.Inputs;

namespace PhoneNumberInput
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            AttachEventHandlers();
        }

        /// <summary>
        /// Attaches event handlers to the masked entry controls to display real-time validation status.
        /// </summary>
        private void AttachEventHandlers()
        {
            if (USPhoneNumberEntry is not null)
            {
                USPhoneNumberEntry.ValueChanged += OnPhoneNumberTextChanged;
            }

            if (InternationalPhoneNumberEntry is not null)
            {
                InternationalPhoneNumberEntry.ValueChanged += OnPhoneNumberTextChanged;
            }

            if (ExtensionPhoneNumberEntry is not null)
            {
                ExtensionPhoneNumberEntry.ValueChanged += OnPhoneNumberTextChanged;
            }
        }

        /// <summary>
        /// Handles text changed event for all masked entry controls.
        /// Updates the status label to show validation state.
        /// </summary>
        private void OnPhoneNumberTextChanged(object? sender, MaskedEntryValueChangedEventArgs e)
        {
            if (sender is SfMaskedEntry maskedEntry)
            {
                // Get the current control based on the sender
                string statusText = GetValidationStatus(maskedEntry.Text);
                
                // Update the corresponding status label
                if (maskedEntry == USPhoneNumberEntry)
                {
                    USPhoneNumberStatus.Text = statusText;
                }
                else if (maskedEntry == InternationalPhoneNumberEntry)
                {
                    InternationalPhoneNumberStatus.Text = statusText;
                }
                else if (maskedEntry == ExtensionPhoneNumberEntry)
                {
                    ExtensionPhoneNumberStatus.Text = statusText;
                }
            }
        }

        /// <summary>
        /// Determines if the phone number is valid based on its format and completeness.
        /// </summary>
        private string GetValidationStatus(string? phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber) || phoneNumber.Contains("_"))
            {
                return "Incomplete - waiting for more input";
            }

            // Remove any whitespace or special formatting for validation
            string cleanNumber = RemoveNonNumeric(phoneNumber);

            if (cleanNumber.Length >= 10) // At least 10 digits
            {
                return $"✓ Valid - {cleanNumber.Length} digits";
            }

            return "Incomplete - waiting for more input";
        }

        /// <summary>
        /// Removes all non-numeric characters from the input string.
        /// </summary>
        private string RemoveNonNumeric(string input)
        {
            return new string(input.Where(char.IsDigit).ToArray());
        }

        /// <summary>
        /// Handles the submit button click event.
        /// Validates all phone numbers and displays the results.
        /// </summary>
        private void OnSubmitClicked(object? sender, EventArgs e)
        {
            // Validate that at least one phone number has been entered
            bool hasUSPhone = !string.IsNullOrEmpty(USPhoneNumberEntry?.Text) && !USPhoneNumberEntry.Text.Contains("_");
            bool hasIntlPhone = !string.IsNullOrEmpty(InternationalPhoneNumberEntry?.Text) && !InternationalPhoneNumberEntry.Text.Contains("_");
            bool hasExtPhone = !string.IsNullOrEmpty(ExtensionPhoneNumberEntry?.Text) && !ExtensionPhoneNumberEntry.Text.Contains("_");

            if (!hasUSPhone && !hasIntlPhone && !hasExtPhone)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert("Validation Error", "Please enter at least one phone number.", "OK");
                });
                return;
            }

            // Build result string
            string result = "Submitted Phone Numbers:\n\n";

            if (hasUSPhone)
            {
                result += $"US Phone: {USPhoneNumberEntry.Text}\n";
            }

            if (hasIntlPhone)
            {
                result += $"International: {InternationalPhoneNumberEntry.Text}\n";
            }

            if (hasExtPhone)
            {
                result += $"India: {ExtensionPhoneNumberEntry.Text}\n";
            }

            // Display the results
            ResultLabel.Text = result;
            ResultFrame.IsVisible = true;

            // Show success message
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("Success", "Phone numbers submitted successfully!", "OK");
            });
        }
    }
}
