# How-to-Build-a-Phone-Number-Input-Form-Using-.NET-MAUI-MaskedEntry

This section explains how to build a phone number input form in .NET MAUI using the SfMaskedEntry control. By applying predefined masks for different phone number formats such as US, International, and India phone numbers, you can ensure that users enter phone numbers in a consistent format while providing real-time validation feedback.

## XAML

Define the SfMaskedEntry controls with appropriate masks to automatically format phone numbers as users enter data. In this example, separate masked entries are used for US, International, and India phone number formats.

```xml
<ScrollView>
    <VerticalStackLayout Padding="20,30"
                         Spacing="20">

        <!-- Title -->
        <Label Text="Phone Number Registration"
               FontSize="28"
               FontAttributes="Bold"
               HorizontalOptions="Center"
               Margin="0,20,0,10"/>

        <Label Text="Enter your phone number using the masked input below"
               FontSize="14"
               TextColor="#666666"
               HorizontalOptions="Center"
               Margin="0,0,0,20"/>

        <!-- US Phone Number Format -->
        <VerticalStackLayout Spacing="8">
            <Label Text="US Phone Number (Format: (XXX) XXX-XXXX)"
                   FontSize="14"
                   FontAttributes="Bold"/>

            <syncfusion:SfMaskedEntry x:Name="USPhoneNumberEntry"
                                      Mask="(###) ###-####"
                                      Placeholder="(123) 456-7890"
                                      HeightRequest="50"
                                      HorizontalTextAlignment="Center"
                                      FontSize="16"
                                      PromptChar="_"/>

            <Label x:Name="USPhoneNumberStatus"
                   FontSize="12"
                   TextColor="#888888"/>
        </VerticalStackLayout>

        <!-- International Phone Number Format -->
        <VerticalStackLayout Spacing="8">
            <Label Text="International Phone Number (Format: +1 (XXX) XXX-XXXX)"
                   FontSize="14"
                   FontAttributes="Bold"/>

            <syncfusion:SfMaskedEntry x:Name="InternationalPhoneNumberEntry"
                                      Mask="+1 (###) ###-####"
                                      Placeholder="+1 (123) 456-7890"
                                      HeightRequest="50"
                                      HorizontalTextAlignment="Center"
                                      FontSize="16"
                                      PromptChar="_"/>

            <Label x:Name="InternationalPhoneNumberStatus"
                   FontSize="12"
                   TextColor="#888888"/>
        </VerticalStackLayout>

        <!-- India Phone Number Format -->
        <VerticalStackLayout Spacing="8">
            <Label Text="India Phone Number (Format: +91 XXXXX XXXXX)"
                   FontSize="14"
                   FontAttributes="Bold"/>

            <syncfusion:SfMaskedEntry x:Name="ExtensionPhoneNumberEntry"
                                      Mask="+91 ##### #####"
                                      Placeholder="+91 98765 43210"
                                      HeightRequest="50"
                                      HorizontalTextAlignment="Center"
                                      FontSize="16"
                                      PromptChar="_"/>

            <Label x:Name="ExtensionPhoneNumberStatus"
                   FontSize="12"
                   TextColor="#888888"/>
        </VerticalStackLayout>

        <!-- Submit Button -->
        <Button x:Name="SubmitButton"
                Text="Submit"
                FontSize="16"
                Padding="20,15"
                Margin="0,20,0,0"
                Clicked="OnSubmitClicked"/>

        <!-- Result Display -->
        <Border x:Name="ResultFrame"
                Stroke="#E0E0E0"
                Padding="15"
                Margin="0,20,0,0"
                IsVisible="False">

            <VerticalStackLayout Spacing="10">
                <Label Text="Submitted Phone Numbers:"
                       FontSize="14"
                       FontAttributes="Bold"/>

                <Label x:Name="ResultLabel"
                       FontSize="13"
                       LineBreakMode="WordWrap"
                       TextColor="#333333"/>
            </VerticalStackLayout>
        </Border>

    </VerticalStackLayout>
</ScrollView>
```

## C#

Handle the ValueChanged event of each SfMaskedEntry control to provide real-time validation feedback. The entered values are validated by checking whether the mask is completely filled and by counting the numeric digits entered. When the form is submitted, the entered phone numbers are displayed in the result section.

```csharp
using Syncfusion.Maui.Inputs;

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

private void OnPhoneNumberTextChanged(object? sender, MaskedEntryValueChangedEventArgs e)
{
    if (sender is SfMaskedEntry maskedEntry)
    {
        string statusText = GetValidationStatus(maskedEntry.Text);

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

private string GetValidationStatus(string? phoneNumber)
{
    if (string.IsNullOrEmpty(phoneNumber) || phoneNumber.Contains("_"))
    {
        return "Incomplete - waiting for more input";
    }

    string cleanNumber = RemoveNonNumeric(phoneNumber);

    if (cleanNumber.Length >= 10)
    {
        return $"✓ Valid - {cleanNumber.Length} digits";
    }

    return "Incomplete - waiting for more input";
}

private string RemoveNonNumeric(string input)
{
    return new string(input.Where(char.IsDigit).ToArray());
}

private void OnSubmitClicked(object? sender, EventArgs e)
{
    bool hasUSPhone = !string.IsNullOrEmpty(USPhoneNumberEntry?.Text) &&
                      !USPhoneNumberEntry.Text.Contains("_");

    bool hasIntlPhone = !string.IsNullOrEmpty(InternationalPhoneNumberEntry?.Text) &&
                        !InternationalPhoneNumberEntry.Text.Contains("_");

    bool hasExtPhone = !string.IsNullOrEmpty(ExtensionPhoneNumberEntry?.Text) &&
                       !ExtensionPhoneNumberEntry.Text.Contains("_");

    if (!hasUSPhone && !hasIntlPhone && !hasExtPhone)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await DisplayAlert(
                "Validation Error",
                "Please enter at least one phone number.",
                "OK");
        });

        return;
    }

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

    ResultLabel.Text = result;
    ResultFrame.IsVisible = true;

    MainThread.BeginInvokeOnMainThread(async () =>
    {
        await DisplayAlert(
            "Success",
            "Phone numbers submitted successfully!",
            "OK");
    });
}
```

The SfMaskedEntry control automatically formats phone numbers according to the specified masks, ensuring consistent user input across different phone number formats. It provides real-time validation feedback, prevents invalid formatting, and simplifies phone number collection by enforcing predefined patterns such as (XXX) XXX-XXXX, +1 (XXX) XXX-XXXX, and +91 XXXXX XXXXX. This helps users enter phone numbers accurately while improving the overall data entry experience.