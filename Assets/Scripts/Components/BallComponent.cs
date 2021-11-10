using System;
using UnityEngine;
using Voody.UniLeo;
using Leopotam.Ecs;

namespace Client
{
    [Serializable]
    public struct BallComponent
    {
        public float Speed;
        public Transform Transform;
        public SpriteRenderer SpriteRenderer;
        public Color Color;

        public int Damage;
        public int Score;
    }
}