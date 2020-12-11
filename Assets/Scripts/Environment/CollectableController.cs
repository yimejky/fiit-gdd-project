using UnityEngine;

public class CollectableController : MonoBehaviour, IInteractableObject
{
    public Reward reward;
    public int rewardAmount = 1;

    Animator animator;
    UIAction uIAction;
    GameObject canvas;

    void Start()
    {
        animator = GetComponent<Animator>();
        uIAction = GetComponent<UIAction>();
        canvas = transform.Find("Canvas").gameObject;
        if (reward == Reward.Random)
        {
            reward = Utils.RandomEnumValue<Reward>();
        }
    }

    public void Interact()
    {
        // FIXME need to do proper logic of handling rewards once more types of rewards will come
        StatsUpgrades.Instance.UpgradeStatWithoutPointLoss(reward.ToString().ToLower(), rewardAmount);
        
        Instantiate(Resources.Load("Animations/Rewards/" + reward.ToString()), (new Vector3(0, 1, 0)), Quaternion.identity, transform);
        
        animator.SetBool("open", true);
        Destroy(uIAction);
        canvas.SetActive(false);
    }
}

public enum Reward
{
    Random, Health, Sword, Bow
}
