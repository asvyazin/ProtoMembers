using System.Collections.Generic;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Macros;

namespace Guybrush.ProtoMembers
{
	[MacroDefinition("pm", ShortDescription = "Add ProtoMember attribute")]
	public class NewProtoMemberMacroDefinition: SimpleMacroDefinition
	{
		public override string GetPlaceholder(IDocument document, IEnumerable<IMacroParameterValue> parameters)
		{
			return "0";
		}
	}
}