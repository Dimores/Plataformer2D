using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "ScriptableObjects/Player")]
public class PlayerData : ScriptableObject
{
    public Animator player;

    [Header("Speed Setup")]
    public FloatData speed;
    public FloatData jumpForce;
    public FloatData timeBetweenShoot;

    [Header("Animation")]
    public FloatData jumpScaleY;
    public FloatData jumpScaleX;
    public FloatData fallScaleY;
    public FloatData fallScaleX;
    public FloatData animationDuration;
    public Ease ease = Ease.OutBack;

    [Header("Animation Control")]
    public StringData boolRun;
    public StringData boolSprint;
    public StringData triggerJump;
    public StringData boolFalling;
    public StringData triggerDeath;
    public FloatData playerSwipeDuration;

    [Header("Inputs")]
    public InputData run;
    public InputData jump;
    public InputData shoot;
    public AxisInputData moveAxis;
}
