using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.ReSharper.Feature.Services;
using JetBrains.ReSharper.Psi.CSharp;

namespace Guybrush.ProtoMembers
{
	[ZoneMarker]
	public class ZoneMarker: IRequire<ILanguageCSharpZone>, IRequire<ICodeEditingZone>
	{
	}
}