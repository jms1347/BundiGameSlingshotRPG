using UnityEngine;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private List<BaseCharacter> partyCharacters = new List<BaseCharacter>();
    private BaseCharacter currentMainCharacter;
    private int currentCharacterIndex = 0;

    public static CharacterManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // �ʱ� ���� ĳ���� ����
        if (partyCharacters.Count > 0)
        {
            SetMainCharacter(partyCharacters[0]);
            // ��� ��Ƽ ĳ���ʹ� �ϴ� ��Ȱ��ȭ ���·� ����
            foreach (var character in partyCharacters)
            {
                character.gameObject.SetActive(false);
            }
            // ù ��° ĳ���͸� Ȱ��ȭ
            currentMainCharacter.gameObject.SetActive(true);
        }
    }

    // ���� ĳ���͸� �����ϰ� UI ���� ������Ʈ�ϴ� �޼���
    private void SetMainCharacter(BaseCharacter character)
    {
        if (currentMainCharacter != null)
        {
            currentMainCharacter.gameObject.SetActive(false); // ���� ���� ĳ���� ��Ȱ��ȭ
        }
        currentMainCharacter = character;
        currentMainCharacter.gameObject.SetActive(true); // �� ���� ĳ���� Ȱ��ȭ
        Debug.Log($"Current main character is: {currentMainCharacter.Name}");

        // ���⿡�� UI ������Ʈ ���� ȣ�� (ĳ���� ������, ü��/������ ��)
        UpdateCharacterUI(currentMainCharacter);

        // �� ��� ĳ���͵��� �нú� ��ų Ȱ��ȭ/��Ȱ��ȭ ó��
        ActivatePetPassiveSkills();
    }

    // ��ü ��ư Ŭ�� �� ȣ��� �޼���
    public void SwitchCharacter()
    {
        if (partyCharacters.Count < 2) return; // ��ü�� ĳ���Ͱ� ������ ����

        // ���� ĳ������ ��ü ���� ��ų �ߵ�
        if (currentMainCharacter != null)
        {
            currentMainCharacter.OnSwitchOutSkill();
        }

        // ���� ĳ���� �ε��� ���
        currentCharacterIndex = (currentCharacterIndex + 1) % partyCharacters.Count;
        BaseCharacter nextCharacter = partyCharacters[currentCharacterIndex];

        // ���ο� ĳ���ͷ� ���� ĳ���� ����
        SetMainCharacter(nextCharacter);

        // ���ο� ĳ������ ��ü ���� ��ų �ߵ�
        nextCharacter.OnSwitchInSkill();
    }

    // �� ��� ĳ���͵��� �нú� ��ų Ȱ��ȭ/��Ȱ��ȭ
    private void ActivatePetPassiveSkills()
    {
        foreach (var character in partyCharacters)
        {
            if (character != currentMainCharacter)
            {
                // �� ����� ĳ���ʹ� �нú� ��ų Ȱ��ȭ
                character.ActivatePassiveSkills();
            }
            //else
            //{
            // ���� ĳ������ �нú�� �⺻������ �׻� Ȱ��ȭ ���·� �����ϰų�
            // �ʿ信 ���� ���� ���� ó��
            //}
        }
    }

    // UI ������Ʈ (����)
    private void UpdateCharacterUI(ICharacter character)
    {
        // ���� ���ӿ����� UI Manager ���� ���� ĳ���� ������, ü��/������ ������Ʈ
        Debug.Log($"UI Updated: Character Icon for {character.Name}, HP: {character.CurrentHP}/{character.MaxHP}, MP: {character.CurrentMP}/{character.MaxMP}");
    }

    // ���� �� ���� ĳ������ ���� ����
    public void GainMPForMainCharacter(int amount)
    {
        if (currentMainCharacter != null)
        {
            currentMainCharacter.GainMP(amount);
        }
    }

    // �ñر� ��� (���� ���� ĳ������ �ñر� ���)
    public void UseMainCharacterUltimate()
    {
        if (currentMainCharacter != null)
        {
            currentMainCharacter.UseUltimateSkill();
        }
    }
}