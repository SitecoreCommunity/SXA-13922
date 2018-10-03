using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;

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
                    // known link types are Hyperlink and RenderRenderingVariantFieldProcessor+RenderFieldControl
                    if (args.ResultControl is Control link && link.Controls.Count == 1)
                    {
                        if (link.Controls[0] is HtmlGenericControl unwantedContainer)
                        {
                            var unwantedTags = new[] { "div", "span" };
                            if (unwantedTags.Any(tag => string.Equals(unwantedContainer.TagName, tag, StringComparison.OrdinalIgnoreCase)))
                            {
                                // act only if dummy <div> or <span> without attributes
                                if (unwantedContainer.Attributes.Count == 0)
                                {
                                    // act only if there are controls in it
                                    if (unwantedContainer.Controls.Count > 0)
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
