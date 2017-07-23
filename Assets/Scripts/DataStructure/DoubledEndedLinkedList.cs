using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleEnded {

    private GameObject _prevComponent;
    private List<GameObject> _nextComponent;
    private GameObject _currentComponent;

    public DoubleEnded(GameObject currentComponent)
    {
        _currentComponent = currentComponent;
    }

	public void SetPreviousComponent(GameObject prevComponent)
    {
        _prevComponent = prevComponent;
    }

    public GameObject GetPreviousComponent()
    {
        return _prevComponent;
    }

    public void SetNextComponent(List<GameObject> nextComponent)
    {
        _nextComponent = nextComponent;
    }

    public List<GameObject> GetNextComponent()
    {
        return _nextComponent;
    }

    public GameObject GetCurrentComponent()
    {
        return _currentComponent;
    }


}
