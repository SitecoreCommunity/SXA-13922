using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Sitecore;
using Sitecore.XA.Foundation.RenderingVariants.Fields;
using Sitecore.XA.Foundation.Variants.Abstractions.Pipelines.RenderVariantField;

namespace SitecoreCommunity.SXA.Foundation.RenderingVariants.Pipelines.RenderVariantField
{
    public class RenderSection : Sitecore.XA.Foundation.RenderingVariants.Pipelines.RenderVariantField.RenderSection
    {
        [UsedImplicitly]
        public RenderSection()
        {
        }

        public override void RenderField(RenderVariantFieldArgs args)
        {
            base.RenderField(args);

            RemoveUnwantedContainer(args);
        }

        private void RemoveUnwantedContainer(RenderVariantFieldArgs args)
        {
            if (args.VariantField is VariantSection variantSection)
            {
                if (variantSection.IsLink && string.IsNullOrWhiteSpace(variantSection.Tag))
                {
                    if (args.ResultControl is HyperLink link && link.Controls.Count == 1)
                    {
                        if (link.Controls[0] is HtmlGenericControl unwantedContainer)
                        {
                            var unwantedTags = new[] { "div" };
                            if (unwantedTags.Any(tag => string.Equals(unwantedContainer.TagName, tag, StringComparison.OrdinalIgnoreCase)))
                            {
                                ReplaceContainerWithItsChildren(unwantedContainer);

                                // re-render control since it's changed
                                args.Result = RenderControl(args.ResultControl);
                            }
                        }
                    }
                }
            }
        }

        internal static void ReplaceContainerWithItsChildren(Control unwantedContainer)
        {
            var control = unwantedContainer.Parent;
            control.Controls.Remove(unwantedContainer);

            var childrenToPreserve = unwantedContainer.Controls
                .Cast<Control>()
                .ToArray();

            foreach (var child in childrenToPreserve)
            {
                control.Controls.Add(child);
            }
        }
    }
}
