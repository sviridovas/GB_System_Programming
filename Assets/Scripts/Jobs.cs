using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.Collections;

public class Jobs : MonoBehaviour
{
    [SerializeField] private Transform[] transforms;
    [SerializeField] float angle = 10;

    private TransformAccessArray _transformAccessArray;

    void Start()
    {
        using var arr = new NativeArray<int>(new []{1, 2, 3, 11, 12, 13}, Allocator.Persistent);
        var myJob = new MyJob() {
            arr = arr
        };

        var jobHandle = myJob.Schedule();
        jobHandle.Complete();
       
        Debug.Log(string.Join(',', myJob.arr ));


        using var positions = new NativeArray<Vector3>(new []{Vector3.up, Vector3.down, Vector3.left, Vector3.right}, Allocator.Persistent);
        using var velocities = new NativeArray<Vector3>(new []{Vector3.up, Vector3.down, Vector3.left, Vector3.right}, Allocator.Persistent);
        using var finalPositions = new  NativeArray<Vector3>(positions.Length, Allocator.Persistent);

        var myarallelForJob = new MyParallelForJob() {
            Positions = positions,
            Velocities = velocities,
            FinalPositions = finalPositions,
        };

        jobHandle = myarallelForJob.Schedule(finalPositions.Length, 0);
        jobHandle.Complete();
       
        Debug.Log(string.Join(',', finalPositions));


        _transformAccessArray = new TransformAccessArray(transforms);
    }

    void Update()
    {
        var myForTransformJob = new MyForTransformJob() {
            angle = angle,
            deltaTime = Time.deltaTime,
        };

        myForTransformJob.Schedule(_transformAccessArray);
    }

    private void OnDestroy()
    {
        _transformAccessArray.Dispose();
    }

    public struct MyJob : IJob
    {
        public NativeArray<int> arr;
        public void Execute()
        {
            for(int i = 0; i != arr.Length; ++i) {
                if(arr[i] > 10) arr[i] = 0;
            }
        }
    }

    public struct MyParallelForJob : IJobParallelFor
    {
        public NativeArray<Vector3> Positions;
        public NativeArray<Vector3> Velocities;
        public NativeArray<Vector3> FinalPositions;
        public void Execute(int index)
        {
            FinalPositions[index] = Positions[index] + Velocities[index];
        }
    }

    public struct MyForTransformJob : IJobParallelForTransform
    {
        public float angle;
        public float deltaTime;
        public void Execute(int index, TransformAccess transform)
        {
            float currentAngle = 0.0f;
            Vector3 axis = Vector3.zero;
            transform.localRotation.ToAngleAxis(out currentAngle, out axis);
            transform.localRotation  = Quaternion.AngleAxis(currentAngle + angle * deltaTime, Vector3.up);
        }
    }

}
