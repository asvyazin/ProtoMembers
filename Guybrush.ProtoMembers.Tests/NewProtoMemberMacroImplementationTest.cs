using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Macros;
using JetBrains.ReSharper.FeaturesTestFramework.LiveTemplates;
using NUnit.Framework;

namespace Guybrush.ProtoMembers.Tests
{
	[TestFixture]
	public class NewProtoMemberMacroImplementationTest: MacroImplTestBase
	{
		protected override IMacroImplementation GetMacro(IEnumerable<IMacroParameterValueNew> parameters)
		{
			throw new NotImplementedException();
		}

		[Test]
		public void Test1()
		{
			DoOneTest("test1");
		}
	}
}