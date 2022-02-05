using NUnit.Framework;
using UnityEngine;

[SetUpFixture]
public class TestSetup
{
	[OneTimeSetUp]
	public void SetUp()
	{
		Debug.unityLogger.logEnabled = false;
	}
}
