using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Core;

namespace PelotonIDE.Presentation
{
    public sealed partial class MainPage : Page
    {
        private void OutputPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            switch (outputPanelPosition)
            {
                case OutputPanelPosition.Bottom:
                    outputPanelTabView.Width = outputPanel.ActualWidth;
                    outputPanelTabView.Height = outputPanel.ActualHeight;
                    outputThumb.Width = outputPanel.ActualWidth;
                    outputThumb.Height = 5;
                    break;
                case OutputPanelPosition.Right:
                    outputPanelTabView.Width = outputPanel.ActualWidth;
                    outputPanelTabView.Height = outputPanel.ActualHeight;
                    outputThumb.Width = 5;
                    outputThumb.Height = outputPanel.ActualHeight;
                    break;
                case OutputPanelPosition.Left:
                    outputPanelTabView.Width = outputPanel.ActualWidth;
                    outputPanelTabView.Height = outputPanel.ActualHeight;
                    outputThumb.Width = 5;
                    outputThumb.Height = outputPanel.ActualHeight;
                    Canvas.SetLeft(outputThumb, outputPanel.ActualWidth - 1);
                    break;
            }
        }

        private void Thumb_DragDelta(object sender, Microsoft.UI.Xaml.Controls.Primitives.DragDeltaEventArgs e)
        {
            double yadjust = outputPanel.Height - e.VerticalChange;
            double xRightAdjust = outputPanel.Width - e.HorizontalChange;
            double xLeftAdjust = outputPanel.Width + e.HorizontalChange;
            if (outputPanelPosition == OutputPanelPosition.Bottom)
            {
                if (yadjust >= 0)
                {
                    outputPanel.Height = yadjust;
                }
            }
            else if (outputPanelPosition == OutputPanelPosition.Left)
            {
                if (xLeftAdjust >= 0)
                {
                    outputPanel.Width = xLeftAdjust;
                }
            }
            else if (outputPanelPosition == OutputPanelPosition.Right)
            {
                if (xRightAdjust >= 0)
                {
                    outputPanel.Width = xRightAdjust;
                }
            }

            if (outputPanelPosition == OutputPanelPosition.Bottom)
            {
                this.ProtectedCursor = InputCursor.CreateFromCoreCursor(new CoreCursor(CoreCursorType.SizeNorthSouth, 0));
            }
            else
            {
                this.ProtectedCursor = InputCursor.CreateFromCoreCursor(new CoreCursor(CoreCursorType.SizeWestEast, 0));
            }
        }

        private void OutputThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            this.ProtectedCursor = InputCursor.CreateFromCoreCursor(new CoreCursor(CoreCursorType.Arrow, 0));
        }

        private async void OutputThumb_PointerEntered(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (outputPanelPosition == OutputPanelPosition.Bottom)
            {
                this.ProtectedCursor = InputCursor.CreateFromCoreCursor(new CoreCursor(CoreCursorType.SizeNorthSouth, 0));
            }
            else
            {
                this.ProtectedCursor = InputCursor.CreateFromCoreCursor(new CoreCursor(CoreCursorType.SizeWestEast, 0));
            }
        }

        private void OutputThumb_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            this.ProtectedCursor = InputCursor.CreateFromCoreCursor(new CoreCursor(CoreCursorType.Arrow, 0));
        }

        private void OutputLeft_Click(object sender, RoutedEventArgs e)
        {
            HandleOutputPanelChange(OutputPanelPosition.Left);
        }

        private void OutputBottom_Click(object sender, RoutedEventArgs e)
        {
            HandleOutputPanelChange(OutputPanelPosition.Bottom);
        }

        private void OutputRight_Click(object sender, RoutedEventArgs e)
        {
            HandleOutputPanelChange(OutputPanelPosition.Right);
        }


    }
}
