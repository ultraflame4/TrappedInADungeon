﻿namespace Core.Entities
{
    public interface IEntityStats
    {
        public float Attack { get; }
        public float Speed { get; }
        public float Defense { get; }
        public float Health { get; }
    }
}