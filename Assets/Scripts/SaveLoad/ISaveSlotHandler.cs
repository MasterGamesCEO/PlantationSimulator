using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveSlotHandler
{
    void LoadSave(int slotIndex);
    void CreateNewSave(int slotIndex);
    void DeleteSave(int slotIndex);
}
