using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/ScriptableObject/VFX Color")]
public class ButtonColorVFX : ScriptableObject
{
    public ParticleSystem colorNormal;
    public ParticleSystem colorFreeze;

    public ParticleSystem colorMoveEffect;
}
