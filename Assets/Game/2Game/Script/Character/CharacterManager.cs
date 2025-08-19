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

        // 초기 메인 캐릭터 설정
        if (partyCharacters.Count > 0)
        {
            SetMainCharacter(partyCharacters[0]);
            // 모든 파티 캐릭터는 일단 비활성화 상태로 시작
            foreach (var character in partyCharacters)
            {
                character.gameObject.SetActive(false);
            }
            // 첫 번째 캐릭터만 활성화
            currentMainCharacter.gameObject.SetActive(true);
        }
    }

    // 메인 캐릭터를 설정하고 UI 등을 업데이트하는 메서드
    private void SetMainCharacter(BaseCharacter character)
    {
        if (currentMainCharacter != null)
        {
            currentMainCharacter.gameObject.SetActive(false); // 이전 메인 캐릭터 비활성화
        }
        currentMainCharacter = character;
        currentMainCharacter.gameObject.SetActive(true); // 새 메인 캐릭터 활성화
        Debug.Log($"Current main character is: {currentMainCharacter.Name}");

        // 여기에서 UI 업데이트 로직 호출 (캐릭터 아이콘, 체력/마나바 등)
        UpdateCharacterUI(currentMainCharacter);

        // 펫 모드 캐릭터들의 패시브 스킬 활성화/비활성화 처리
        ActivatePetPassiveSkills();
    }

    // 교체 버튼 클릭 시 호출될 메서드
    public void SwitchCharacter()
    {
        if (partyCharacters.Count < 2) return; // 교체할 캐릭터가 없으면 리턴

        // 현재 캐릭터의 교체 퇴장 스킬 발동
        if (currentMainCharacter != null)
        {
            currentMainCharacter.OnSwitchOutSkill();
        }

        // 다음 캐릭터 인덱스 계산
        currentCharacterIndex = (currentCharacterIndex + 1) % partyCharacters.Count;
        BaseCharacter nextCharacter = partyCharacters[currentCharacterIndex];

        // 새로운 캐릭터로 메인 캐릭터 설정
        SetMainCharacter(nextCharacter);

        // 새로운 캐릭터의 교체 등장 스킬 발동
        nextCharacter.OnSwitchInSkill();
    }

    // 펫 모드 캐릭터들의 패시브 스킬 활성화/비활성화
    private void ActivatePetPassiveSkills()
    {
        foreach (var character in partyCharacters)
        {
            if (character != currentMainCharacter)
            {
                // 펫 모드인 캐릭터는 패시브 스킬 활성화
                character.ActivatePassiveSkills();
            }
            //else
            //{
            // 메인 캐릭터의 패시브는 기본적으로 항상 활성화 상태로 가정하거나
            // 필요에 따라 별도 로직 처리
            //}
        }
    }

    // UI 업데이트 (예시)
    private void UpdateCharacterUI(ICharacter character)
    {
        // 실제 게임에서는 UI Manager 등을 통해 캐릭터 아이콘, 체력/마나바 업데이트
        Debug.Log($"UI Updated: Character Icon for {character.Name}, HP: {character.CurrentHP}/{character.MaxHP}, MP: {character.CurrentMP}/{character.MaxMP}");
    }

    // 전투 중 메인 캐릭터의 마나 증가
    public void GainMPForMainCharacter(int amount)
    {
        if (currentMainCharacter != null)
        {
            currentMainCharacter.GainMP(amount);
        }
    }

    // 궁극기 사용 (현재 메인 캐릭터의 궁극기 사용)
    public void UseMainCharacterUltimate()
    {
        if (currentMainCharacter != null)
        {
            currentMainCharacter.UseUltimateSkill();
        }
    }
}