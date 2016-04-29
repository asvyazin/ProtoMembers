using System.Collections.Generic;
using JetBrains.Application.platforms;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Macros;
using JetBrains.ReSharper.FeaturesTestFramework.LiveTemplates;
using JetBrains.Util;
using NUnit.Framework;

namespace Guybrush.ProtoMembers.Tests
{
	[TestFixture]
	public class NewProtoMemberMacroImplementationTest : MacroImplTestBase
	{
		protected override IMacroImplementation GetMacro(IEnumerable<IMacroParameterValueNew> parameters)
		{
			return new NewProtoMemberMacroImplementation();
		}

		protected override IEnumerable<string> GetReferencedAssemblies(PlatformID platform)
		{
			return base.GetReferencedAssemblies(platform).Concat("protobuf-net.dll");
		}

		[Test]
		public void Test1()
		{
			DoOneTest("test1");
		}

		[Test]
		public void Test2()
		{
			DoOneTest("test2");
		}
	}
}