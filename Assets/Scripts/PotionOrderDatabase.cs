using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Potions/Potion Order Database", fileName = "New Potion Order Database")]
public class PotionOrderDatabase : ScriptableObject
{
   public List<PotionOrder> potionOrders;
}
