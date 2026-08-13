using System.Collections;
using UnityEngine;

public class Slipstream : Item
{
    private static WaitForSeconds _waitForSeconds10 = new(10f);
    [SerializeField] private float _playerTime = 1.75f;
    [SerializeField] private float _otherTime = 0.1f;

    private int _itemUses = 0;
    private bool _inUse = false;
    void OnEnable()
    {
        _itemUses = _itemSO.ItemUsage;
    }
    public override void UseItem()
    {
        //Double use stop check
        if(_inUse) return;

        if(_itemUses <= 0) return;

        StartCoroutine(UseSlipstream());
        _itemUses -= 1;
    }

    private IEnumerator UseSlipstream()
    {
        _inUse = true;
        if(_timeManager != null)
        _timeManager.ChangeGameTime(_playerTime,_otherTime);

        yield return _waitForSeconds10;

        //Changes everything back to default time
        _inUse = false;
        if(_timeManager != null)
        _timeManager.ChangeGameTime(1f);
        
    }

}
