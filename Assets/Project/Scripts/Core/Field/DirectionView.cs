using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Core.Field
{
    public enum Directions
    {
        Current,
        Left,
        Right,
        Up,
        Down,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight
    }

    [Serializable]
    public class Direction
    {
        [SerializeField]
        private Directions _direction;

        [SerializeField]
        private GameObject _view;

        [SerializeField]
        private bool _flipX;

        [SerializeField]
        private bool _flipY;

        public Directions Direction1 => _direction;

        public GameObject View => _view;

        public bool FlipX => _flipX;

        public bool FlipY => _flipY;
    }

    public class DirectionView : MonoBehaviour
    {
        [SerializeField]
        private List<Direction> _directions = new();

        private Transform _transform;

        public Transform Transform => _transform ??= transform;

        public void Apply(Vector2 vector)
        {
            if (vector == Vector2.up)
            {
                ApplyView(Directions.Up);
                return;
            }

            if (vector == Vector2.down)
            {
                ApplyView(Directions.Down);
                return;
            }

            if (vector == Vector2.right)
            {
                ApplyView(Directions.Right);
                return;
            }

            if (vector == Vector2.left)
            {
                ApplyView(Directions.Left);
                return;
            }

            if (vector.x > 0 && vector.y > 0)
            {
                ApplyView(Directions.UpRight);
                return;
            }

            if (vector.x > 0 && vector.y < 0)
            {
                ApplyView(Directions.DownRight);
                return;
            }

            if (vector.x < 0 && vector.y > 0)
            {
                ApplyView(Directions.UpLeft);
                return;
            }

            if (vector.x < 0 && vector.y < 0)
            {
                ApplyView(Directions.DownLeft);
                return;
            }

            ApplyView(Directions.Current);
        }

        private void ApplyView(Directions direction)
        {
            foreach (var d in _directions)
            {
                d.View.SetActive(false);
            }

            var dir = _directions.Find(x => x.Direction1 == direction);
            dir.View.SetActive(true);
            var scale = Transform.localScale;
            Transform.localScale = new Vector3((dir.FlipX ? -1 : 1) * Mathf.Abs(scale.x), (dir.FlipY ? -1 : 1) *  Mathf.Abs(scale.y), scale.z);
        }
    }
}