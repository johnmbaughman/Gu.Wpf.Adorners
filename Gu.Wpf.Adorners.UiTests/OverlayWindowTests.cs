namespace Gu.Wpf.Adorners.UiTests
{
    using System;
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public class OverlayWindowTests
    {
        private const string ExeFileName = "Gu.Wpf.Adorners.Demo.exe";
        private const string WindowName = "OverlayWindow";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            ImageAssert.OnFail = OnFail.SaveImageToTemp;
        }

        [TestCase("No overlay", ".\\Images\\No overlay.png")]
        [TestCase("Default visibility", ".\\Images\\Default visibility.png")]
        [TestCase("With content template", ".\\Images\\With content template.png")]
        public void Overlay(string name, string imageFileName)
        {
            using (var app = Application.Launch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                Wait.For(TimeSpan.FromMilliseconds(200));
                var button = window.FindButton(name);
                ImageAssert.AreEqual(imageFileName, button);
            }
        }

        [Test]
        public void BoundVisibility()
        {
            using (var app = Application.Launch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                Wait.For(TimeSpan.FromMilliseconds(200));
                var button = window.FindButton("Bound visibility");
                ImageAssert.AreEqual(".\\Images\\Bound visibility_visible.png", button);

                window.FindToggleButton("IsVisibleButton").IsChecked = false;
                ImageAssert.AreEqual(".\\Images\\Bound visibility_not_visible.png", button);
            }
        }

        [Test]
        public void WithInheritedContentTemplate()
        {
            using (var app = Application.Launch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                Wait.For(TimeSpan.FromMilliseconds(200));
                var groupBox = window.FindGroupBox("Inherits");
                ImageAssert.AreEqual(".\\Images\\WithInheritedContentTemplate.png", groupBox);
            }
        }

        [Test]
        public void WhenSizeChanges()
        {
            using (var app = Application.Launch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                Wait.For(TimeSpan.FromMilliseconds(200));
                var button = window.FindButton("Default visibility");
                ImageAssert.AreEqual(".\\Images\\Default visibility.png", button);

                window.FindSlider("WidthSlider").Value = 100;
                ImageAssert.AreEqual(".\\Images\\Default visibility_width_100.png", button);
            }
        }

        [TestCase("Collapsed")]
        [TestCase("Hidden")]
        public void WhenAdornedElementIs(string visibility)
        {
            using (var app = Application.Launch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                Wait.For(TimeSpan.FromMilliseconds(200));
                var button = window.FindButton("Default visibility");
                ImageAssert.AreEqual(".\\Images\\Default visibility.png", button);

                var comboBox = window.FindComboBox("VisibilityCbx");
                comboBox.Select(visibility);
                Wait.For(TimeSpan.FromMilliseconds(200));

                // Checking that we don't crash here. See issue #24
                comboBox.Select("Visible");
                Wait.For(TimeSpan.FromMilliseconds(200));
                ImageAssert.AreEqual(".\\Images\\Default visibility.png", button);
            }
        }
    }
}
