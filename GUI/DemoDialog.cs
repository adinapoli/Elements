using System;
using System.Collections.Generic;

using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;

namespace Nuclex.UserInterface.ElementsGUI {

  /// <summary>Dialog that demonstrates the capabilities of the GUI library</summary>
  public partial class DemoDialog : WindowControl {

    /// <summary>Initializes a new GUI demonstration dialog</summary>
    public DemoDialog() {
      InitializeComponent();
    }
    
    /// <summary>Called when the user clicks on the okay button</summary>
    /// <param name="sender">Button the user has clicked on</param>
    /// <param name="arguments">Not used</param>
    private void okClicked(object sender, EventArgs arguments) {
      Close();
    }

    /// <summary>Called when the user clicks on the cancel button</summary>
    /// <param name="sender">Button the user has clicked on</param>
    /// <param name="arguments">Not used</param>
    private void cancelClicked(object sender, EventArgs arguments) {
      Close();
    }

  }

} // namespace Nuclex.UserInterface.Demo
