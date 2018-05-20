using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JobLiteBuildWalls : JobLite {

    public ResourcesLoading.TileBasesName nameOfTile;

    // to be deleted ?
    public JobLiteBuildWalls(Vector3 position, string nameOfWorker, string typeOfJob, float timeWorked, ResourcesLoading.TileBasesName nameOfTile) : base(position, nameOfWorker, typeOfJob, timeWorked)
    {
        this.nameOfTile = nameOfTile;
    }

    public JobLiteBuildWalls(Job j, ResourcesLoading.TileBasesName nameOfTile) : base(j)
    {
        this.nameOfTile = nameOfTile;
    }

}
