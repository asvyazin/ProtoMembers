using System.Collections.Generic;
using System.Reflection;
using Guybrush.ProtoMembers;
using JetBrains.Application;
using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.ReSharper.Resources.Shell;
using JetBrains.TestFramework;
using JetBrains.TestFramework.Application.Zones;
using JetBrains.Threading;
using NUnit.Framework;

[SetUpFixture]
public class ProtoMembersTestEnvironmentAssembly : TestEnvironmentAssembly<TestEnvironmentZone>
{
	private static IEnumerable<Assembly> GetAssembliesToLoad()
	{
		// Test assembly
		yield return Assembly.GetExecutingAssembly();
		yield return typeof (NewProtoMemberMacroImplementation).Assembly;
	}

	public override void SetUp()
	{
		base.SetUp();
		ReentrancyGuard.Current.Execute(
			"LoadAssemblies",
			() => Shell.Instance.GetComponent<AssemblyManager>().LoadAssemblies(
				GetType().Name,
				GetAssembliesToLoad()));
	}

	public override void TearDown()
	{
		ReentrancyGuard.Current.Execute(
			"UnloadAssemblies",
			() => Shell.Instance.GetComponent<AssemblyManager>().UnloadAssemblies(
				GetType().Name,
				GetAssembliesToLoad()));
		base.TearDown();
	}
}

[ZoneDefinition]
public class TestEnvironmentZone : ITestsZone
{
}