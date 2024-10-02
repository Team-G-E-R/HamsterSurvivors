using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IExperienceObserver
{
    void OnExpTakingRangeChange(float newExpTakingRange);

    void OnExpMultiplierChange(float newExpMultiplier);
}


