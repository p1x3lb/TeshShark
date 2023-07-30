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
            }

            if (vector == Vector2.down)
            {
                ApplyView(Directions.Down);
            }

            if (vector == Vector2.right)
            {
                ApplyView(Directions.Right);
            }

            if (vector == Vector2.left)
            {
                ApplyView(Directions.Left);
            }

            if (vector.x > 0 && vector.y > 0)
            {
                ApplyView(Directions.UpRight);
            }

            if (vector.x > 0 && vector.y < 0)
            {
                ApplyView(Directions.DownRight);
            }

            if (vector.x < 0 && vector.y > 0)
            {
                ApplyView(Directions.UpLeft);
            }

            if (vector.x < 0 && vector.y < 0)
            {
                ApplyView(Directions.DownLeft);
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
            Transform.localScale = new Vector3((dir.FlipX ? -1 : 1) * Mathf.Abs(Transform.localScale.x), (dir.FlipY ? -1 : 1) *  Mathf.Abs(Transform.localScale.y), Transform.localScale.z);
        }
    }
}