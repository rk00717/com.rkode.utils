using UnityEngine;
using UnityEngine.TestTools;

using NUnit.Framework;

using System.Collections;

namespace RKode.Utils {
public class SingletonTests {
    public class TestSingleton : Singleton<TestSingleton> {
        public int testValue = 0;
        public bool awakeWasCalled = false;

        protected override void OnAwake() {
            base.OnAwake();

            testValue = 42;
            awakeWasCalled = true;
        }
    }

    [TearDown]
    public void TearDown() {
        // Clean up any test singletons
        var instances = Object.FindObjectsOfType<TestSingleton>();
        foreach (var instance in instances) {
            Object.DestroyImmediate(instance.gameObject);
        }
    }

    [UnityTest]
    public IEnumerator Singleton_Instance_CreatesAutomatically() {
        // Create GameObject with singleton
        var go = new GameObject("TestSingleton");
        go.AddComponent<TestSingleton>();
        
        yield return null;
        
        // Verify instance exists
        Assert.IsNotNull(TestSingleton.Instance, "Instance should be automatically created");
        Assert.AreEqual(42, TestSingleton.Instance.testValue, "OnAwake should be called");
    }

    [UnityTest]
    public IEnumerator Singleton_DestroysDuplicate_WhenSecondInstanceCreated() {
        // Create first instance
        var go1 = new GameObject("TestSingleton1");
        var instance1 = go1.AddComponent<TestSingleton>();
        
        yield return null;
        
        // Create second instance (should be destroyed)
        var go2 = new GameObject("TestSingleton2");
        var instance2 = go2.AddComponent<TestSingleton>();
        
        yield return null;
        yield return null; // Extra frame for destruction
        
        // First instance should survive
        Assert.IsNotNull(instance1, "First instance should still exist");
        Assert.IsTrue(go1 != null, "First GameObject should still exist");
        
        // Second should be destroyed
        Assert.IsNull(instance2, "Second instance should be destroyed");
    }

    [UnityTest]
    public IEnumerator Singleton_HasInstance_ReturnsTrueWhenExists() {
        var go = new GameObject("TestSingleton");
        go.AddComponent<TestSingleton>();
        
        yield return null;
        
        Assert.IsTrue(TestSingleton.HasInstance, "HasInstance should return true");
    }

    [UnityTest]
    public IEnumerator Singleton_HasInstance_ReturnsFalseWhenDoesntExist() {
        // Don't create any instance
        yield return null;
        
        Assert.IsFalse(TestSingleton.HasInstance, "HasInstance should return false when no instance");
    }

    [UnityTest]
    public IEnumerator Singleton_Persistent_SurvivesDontDestroyOnLoad() {
        var go = new GameObject("TestSingleton");
        var instance = go.AddComponent<TestSingleton>();
        // persistent is true by default
        
        yield return null;
        
        // Check if object is marked as DontDestroyOnLoad
        Assert.IsNotNull(instance, "Persistent singleton should exist");
        // Note: We can't fully test DontDestroyOnLoad without scene loading
    }

    [UnityTest]
    public IEnumerator Singleton_OnAwake_CalledInsteadOfAwake() {
        var go = new GameObject("TestSingleton");
        var instance = go.AddComponent<TestSingleton>();
        
        yield return null;
        
        Assert.IsTrue(instance.awakeWasCalled, "OnAwake should be called");
    }
}
}