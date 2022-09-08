using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FloorPlanning 
{
    [Serializable]
    public class DrawingOp
    {
        OpType opType;
        DrawingEntity entity;
        DrawingEntity oldEntity;
        bool isBasePlan;
        
        public enum OpType
        {
            /// <summary>
            /// Add
            /// </summary>
            Add = 0,

            /// <summary>
            /// DeleteSelected
            /// </summary>
            Delete = 1,

            /// <summary>
            /// DeleteSelected
            /// </summary>
            TempDelete = 2,

            /// <summary>
            /// Add
            /// </summary>
            Replace = 3,

            /// <summary>
            /// TempDeleteBasePlan
            /// </summary>
            TempDeleteBasePlan = 4,
            
            /// <summary>
            /// AddBasePlan
            /// </summary>
            AddBasePlan = 5,

            /// <summary>
            /// Resize
            /// </summary>
            ReplaceBasePlan = 6,
        }

        // Constructor for Add, DeleteSelected, and TempDelete
        public DrawingOp(OpType opType, DrawingEntity entity)
        {
            this.opType = opType;
            this.entity = entity;
            oldEntity = null;

            if (opType == OpType.Delete)
                entity.DisposeImages();
        }

        // Constructor for AddBasePlan
        public DrawingOp(OpType opType, DrawingEntity entity, bool isBasePlan)
        {
            this.opType = opType;
            this.entity = entity;
            this.isBasePlan = opType == OpType.AddBasePlan && isBasePlan;
            oldEntity = null;

            if (opType == OpType.Delete)
                entity.DisposeImages();
        }
        
        // Constructor For Replace
        public DrawingOp(OpType opType, DrawingEntity entity, DrawingEntity oldEntity)
        {
            this.opType = opType;
            this.entity = entity;
            this.oldEntity = oldEntity;
            oldEntity.DisposeImages();
        }

        // Constructor For ReplaceBasePlan
        public DrawingOp(OpType opType, DrawingEntity entity, DrawingEntity oldEntity, bool isBasePlan)
        {
            this.opType = opType;
            this.entity = entity;
            this.oldEntity = oldEntity;
            this.isBasePlan = opType == OpType.AddBasePlan && isBasePlan;
            oldEntity.DisposeImages();
        }

        // Properties
        //
        public OpType Type
        {
            get { return opType; }
        }

        public DrawingEntity Entity
        {
            get { return entity; }
            set
            {
                entity = value;
            }
        }

        public DrawingEntity OldEntity
        {
            get { return oldEntity; }
        }

        public void Dereference()
        {
            if (opType != OpType.Delete)
                if(entity != null)
                {
                    entity.Dispose();
                }
        }
    }
}
