using System;
using UnityEngine;

namespace ARPG
{
    public class TestArchitecture : MonoBehaviour
    {
        private void Start()
        {
            for (int i = 0; i < 5; i++)
            {
                var test = new TestActor();
                test.Init();
                var model = test.GetModel<ITestModel>();
                print("A is " + model.A);
            }
        }
    }

    public class TestActor : AbstractActor<TestActor>
    {
        protected override void OnInit()
        {
            RegisterModel<ITestModel>(new TestModel());
            UnityEngine.Debug.Log("TestActor.OnInit");
        }
    }

    public interface ITestModel : IModel
    {
        public bool A { get; set; }
    }

    public class TestModel : AbstractModel, ITestModel, ICanGetModel
    {
        protected override void OnInit()
        {
            UnityEngine.Debug.Log("TestModel.OnInit");
        }

        public bool A { get; set; } = false;
    }
}