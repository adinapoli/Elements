using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Input;

namespace Nuclex.UserInterface.ElementsGUI {

  partial class DemoDialog {

    #region NOT Component Designer generated code

    /// <summary> 
    ///   Required method for user interface initialization -
    ///   do modify the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.nameEntryLabel = new Nuclex.UserInterface.Controls.LabelControl();
      this.nameEntryBox = new Nuclex.UserInterface.Controls.Desktop.InputControl();
      this.rememberOption = new Nuclex.UserInterface.Controls.Desktop.OptionControl();
      this.easyChoice = new Nuclex.UserInterface.Controls.Desktop.ChoiceControl();
      this.normalChoice = new Nuclex.UserInterface.Controls.Desktop.ChoiceControl();
      this.hardChoice = new Nuclex.UserInterface.Controls.Desktop.ChoiceControl();
      this.mapLabel = new Nuclex.UserInterface.Controls.LabelControl();
      this.mapList = new Nuclex.UserInterface.Controls.Desktop.ListControl();
      this.okButton = new Nuclex.UserInterface.Controls.Desktop.ButtonControl();
      this.cancelButton = new Nuclex.UserInterface.Controls.Desktop.ButtonControl();
      //
      // nameEntryLabel
      //
      this.nameEntryLabel.Text = "Please enter your name:";
      this.nameEntryLabel.Bounds = new UniRectangle(10.0f, 30.0f, 110.0f, 24.0f);
      //
      // nameEntryBox
      //
      this.nameEntryBox.Bounds = new UniRectangle(10.0f, 55.0f, 492.0f, 24.0f);
      this.nameEntryBox.Text = "John Doe";
      //
      // rememberOption
      //
      this.rememberOption.Bounds = new UniRectangle(10.0f, 95.0f, 170.0f, 16.0f);
      this.rememberOption.Text = "Remember my name";
      //
      // easyChoice
      //
      this.easyChoice.Bounds = new UniRectangle(10.0f, 125.0f, 120.0f, 16.0f);
      this.easyChoice.Text = "Easy";
      //
      // normalChoice
      //
      this.normalChoice.Bounds = new UniRectangle(10.0f, 145.0f, 120.0f, 16.0f);
      this.normalChoice.Text = "Normal";
      this.normalChoice.Selected = true;
      //
      // hardChoice
      //
      this.hardChoice.Bounds = new UniRectangle(10.0f, 165.0f, 120.0f, 16.0f);
      this.hardChoice.Text = "Hard";
      //
      // mapLabel
      //
      this.mapLabel.Bounds = new UniRectangle(10.0f, 190.0f, 200.0f, 24.0f);
      this.mapLabel.Text = "Which map do you want to play?";
      //
      // mapList
      //
      this.mapList.Bounds = new UniRectangle(10.0f, 215.0f, 492.0f, 120.0f);
      this.mapList.Slider.Bounds.Location.X.Offset -= 1.0f;
      this.mapList.Slider.Bounds.Location.Y.Offset += 1.0f;
      this.mapList.Slider.Bounds.Size.Y.Offset -= 2.0f;
      this.mapList.Items.Add("What Map?");
      this.mapList.Items.Add("No Map");
      this.mapList.Items.Add("Any Map");
      this.mapList.Items.Add("Episode 1 - No Map's Land");
      this.mapList.Items.Add("Episode 2 - The Mappet Show");
      this.mapList.Items.Add("Episode 3 - Staring Down the Map");
      this.mapList.Items.Add("Episode 4 - Map Cemetary");
      this.mapList.Items.Add("Episode 5 - Kidmapped");
      this.mapList.Items.Add("Episode 6 - The Mapchurian Candidate");
      this.mapList.Items.Add("Yes");
      this.mapList.SelectionMode =
        Nuclex.UserInterface.Controls.Desktop.ListSelectionMode.Single;
      this.mapList.SelectedItems.Add(3);
      //
      // okButton
      //
      this.okButton.Bounds = new UniRectangle(
        new UniScalar(1.0f, -180.0f), new UniScalar(1.0f, -40.0f), 80, 24
      );
      this.okButton.Text = "Ok";
      this.okButton.ShortcutButton = Buttons.A;
      this.okButton.Pressed += new EventHandler(okClicked);
      //
      // cancelButton
      //
      this.cancelButton.Bounds = new UniRectangle(
        new UniScalar(1.0f, -90.0f), new UniScalar(1.0f, -40.0f), 80, 24
      );
      this.cancelButton.Text = "Cancel";
      this.cancelButton.ShortcutButton = Buttons.B;
      this.cancelButton.Pressed += new EventHandler(cancelClicked);
      //
      // DemoDialog
      //
      this.Bounds = new UniRectangle(80.0f, 10.0f, 512.0f, 384.0f);
      this.Title = "Demo Dialog";
      Children.Add(this.nameEntryLabel);
      Children.Add(this.nameEntryBox);
      Children.Add(this.rememberOption);
      Children.Add(this.easyChoice);
      Children.Add(this.normalChoice);
      Children.Add(this.hardChoice);
      Children.Add(this.mapLabel);
      Children.Add(this.mapList);
      Children.Add(this.okButton);
      Children.Add(this.cancelButton);
    }

    #endregion // NOT Component Designer generated code

    /// <summary>A label used ask the user to enter his name</summary>
    private Nuclex.UserInterface.Controls.LabelControl nameEntryLabel;
    /// <summary>Text input control where the user can enter his name</summary>
    private Nuclex.UserInterface.Controls.Desktop.InputControl nameEntryBox;
    /// <summary>Option the user can select to have his name remembered</summary>
    private Nuclex.UserInterface.Controls.Desktop.OptionControl rememberOption;
    /// <summary>Choice for the easy difficulty level</summary>
    private Nuclex.UserInterface.Controls.Desktop.ChoiceControl easyChoice;
    /// <summary>Choice for the normal difficulty level</summary>
    private Nuclex.UserInterface.Controls.Desktop.ChoiceControl normalChoice;
    /// <summary>Choice for the hard difficulty level</summary>
    private Nuclex.UserInterface.Controls.Desktop.ChoiceControl hardChoice;
    /// <summary>A label used ask the user to select a map</summary>
    private Nuclex.UserInterface.Controls.LabelControl mapLabel;
    /// <summary>List of maps the player can select from</summary>
    private Nuclex.UserInterface.Controls.Desktop.ListControl mapList;
    /// <summary>Button which exits the dialog and takes over the settings</summary>
    private Nuclex.UserInterface.Controls.Desktop.ButtonControl okButton;
    /// <summary>Button which exits the dialog and discards the settings</summary>
    private Nuclex.UserInterface.Controls.Desktop.ButtonControl cancelButton;

  }

} // namespace Nuclex.UserInterface.Demo
