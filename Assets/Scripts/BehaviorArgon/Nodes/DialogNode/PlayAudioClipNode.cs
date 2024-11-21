using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioClipNode : NodeBeh
{
    public override void Init(params object[] vs)
    {
        AddParameter(0, "Name", StringTypePS, vs);
        AddParameter(1, "Source", AudioSourceTypePS, vs);
    }

    public override void OnStart()
    {
        AudioSource source = InterGetParameter<AudioSource>("Source");
        string name_clip = InterGetParameter<string>("Name");
        AudioClip clip = BaseFunc.GetAudioClipPrefab(name_clip);
        source.PlayOneShot(clip);
    }

    public override void OnUpdate()
    {
        
    }

    public override TaskResult TaskUpdate()
    {
        return TaskResult.COMPLETE;
    }
    public static NodeBeh AddNode<T>(T value, BehaviorExecutor be, string Name, AudioSource source) where T : NodeBeh
    {
        T node = be.gameObject.AddComponent<T>();
        node.InitBase(be.tree, be.nodeIstance, Name,source);

        be.nodeIstance.ReParent(node);
        be.nodes.Add(node);
        return node;

    }
    public PlayAudioClipNode(BehaviorExecutor be, string Name,AudioSource source)
    {
        AddNode(this, be, Name,source);
    }

}
