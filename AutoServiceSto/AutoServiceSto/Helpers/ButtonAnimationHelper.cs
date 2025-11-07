using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace AutoServiceSto.Helpers
{
    public static class ButtonAnimationHelper
    {
        public static void PlayClickAnimation(FrameworkElement element)
        {
            var scaleAnimation = new DoubleAnimation
            {
                To = 0.95,
                Duration = TimeSpan.FromMilliseconds(100),
                AutoReverse = true
            };

            var storyboard = new Storyboard();
            storyboard.Children.Add(scaleAnimation);
            Storyboard.SetTarget(scaleAnimation, element);
            Storyboard.SetTargetProperty(scaleAnimation, new PropertyPath("RenderTransform.ScaleX"));

            var scaleYAnimation = scaleAnimation.Clone();
            Storyboard.SetTarget(scaleYAnimation, element);
            Storyboard.SetTargetProperty(scaleYAnimation, new PropertyPath("RenderTransform.ScaleY"));
            storyboard.Children.Add(scaleYAnimation);

            // Убедимся что есть Transform
            if (element.RenderTransform == null || !(element.RenderTransform is ScaleTransform))
            {
                element.RenderTransform = new ScaleTransform(1, 1);
                element.RenderTransformOrigin = new Point(0.5, 0.5);
            }

            storyboard.Begin();
        }

        public static void PlaySuccessAnimation(FrameworkElement element)
        {
            var colorAnimation = new ColorAnimation
            {
                To = System.Windows.Media.Color.FromRgb(76, 175, 80),
                Duration = TimeSpan.FromMilliseconds(300),
                AutoReverse = true
            };

            var storyboard = new Storyboard();
            storyboard.Children.Add(colorAnimation);
            Storyboard.SetTarget(colorAnimation, element);
            Storyboard.SetTargetProperty(colorAnimation, new PropertyPath("Background.Color"));

            storyboard.Begin();
        }
    }
}