using GTA;
using GTA.Native;
using System;


public class Main : Script
{
    public Prop WeaponOnBack;
    bool attached = false;

    private enum BigWeapons : uint
    {
        SniperRifle = 100416529, // 0x05FC3C11
                                 // 0x060EC506
        CombatPDW = 171789620, // 0x0A3D4D34
        HeavySniper = 205991906, // 0x0C472FE2
        PumpShotgun = 487013001, // 0x1D073A89
        HeavyShotgun = 984333226, // 0x3AABBBAA
        Minigun = 1119849093, // 0x42BF8A85
        GrenadeLauncherSmoke = 1305664598, // 0x4DD2DC56
        Gusenberg = 1627465347, // 0x61012683
        CompactRifle = 1649403952, // 0x624FE830
        HomingLauncher = 1672152130, // 0x63AB0442
        Railgun = 1834241177, // 0x6D544C99
        SawnOffShotgun = 2017895192, // 0x7846A318
        BullpupRifle = 2132975508, // 0x7F229F94
        Firework = 2138347493, // 0x7F7497E5
        CombatMG = 2144741730, // 0x7FD62962
        CarbineRifle = 2210333304, // 0x83BF0278
        MG = 2634544996, // 0x9D07F764
        BullpupShotgun = 2640438543, // 0x9D61E50F
        GrenadeLauncher = 2726580491, // 0xA284510B
        Musket = 2828843422, // 0xA89CB99E
        AdvancedRifle = 2937143193, // 0xAF113F99
        RPG = 2982836145, // 0xB1CA77B1
        AssaultRifle = 3220176749, // 0xBFEFFF6D
        SpecialCarbine = 3231910285, // 0xC0A3098D
        MarksmanRifle = 3342088282, // 0xC734385A
        AssaultShotgun = 3800352039, // 0xE284C527
        DoubleBarrelShotgun = 4019527611, // 0xEF951FBB
        AssaultSMG = 4024951519, // 0xEFE7E2DF
        SMG = 736523883, // 0x2BE6766B
        MicroSMG = 324215364, // 0x13532244
        CarbineRifleMKII = 0xFAD1F1C9,
        SpecialCarbineMKII = 0x969C3D67,
        AssaultRifleMKII = 0x394F415C,
        BullupRifleMKII = 0x84D6FAFD,
        CombatMGMKII = 0xDBBD7280,
        HeavySniperMKII = 0xA914799,
        MarksManRifleMKII = 0x6A6C02E0,
        WidowMaker = 0xB62D1F67,
        SmgMKII = 0x78A97CD0,
        RayCarbine = 0x476BF155,
        SweeperShotgun = 0x12E82D3D,
        PumpShotgunMKII = 0x555AF99A,
        CompactLauncher = 0x0781FE4A,
        MiniSMG = 0xBD248B55,
    }




    public bool on = false;
    public Main()
    {
        this.Tick += new EventHandler(this.OnTick);

    }

    public static int GetBigWeaponCount(Ped ped)
    {
        int num = 0;
        foreach (BigWeapons bigWeapons in Enum.GetValues(typeof(BigWeapons)))
        {
            if (Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON, (InputArgument)ped.Handle, (InputArgument)bigWeapons.GetHashCode(), (InputArgument)false))
                ++num;
        }
        return num;
    }
    Prop current1 = null;
    private void OnTick(object sender, EventArgs e)
    {

        if (GetBigWeaponCount(Game.Player.Character) >= 1)
        {


            if (Function.Call<bool>(Hash.IS_PED_ARMED, (InputArgument)Game.Player.Character, (InputArgument)7))
            {

                current1 = Game.Player.Character.Weapons.CurrentWeaponObject;
            }
            Model model = Game.Player.Character.Weapons.Current.Model;
            WeaponHash current = Game.Player.Character.Weapons.Current.Hash;
            int hand = Function.Call<int>(Hash.GET_PED_BONE_INDEX, (InputArgument)((Entity)Game.Player.Character), (InputArgument)Bone.SKEL_L_Hand);

            if ((Game.Player.Character.IsClimbing || Game.Player.Character.IsSwimming || Game.Player.Character.IsSwimmingUnderWater || Game.Player.Character.IsInAir) && !current1.IsVisible)
            {

                attached = true;

            }
            else
            {

                attached = false;
            }
            if (attached && WeaponOnBack == null && !this.on)
            {

                this.WeaponOnBack = World.CreateProp(model, Game.Player.Character.Position, false, false);
                int spine = Function.Call<int>(Hash.GET_PED_BONE_INDEX, (InputArgument)((Entity)Game.Player.Character), (InputArgument)Bone.SKEL_Spine3);
                Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, (InputArgument)((Entity)WeaponOnBack), (InputArgument)((Entity)Game.Player.Character), (InputArgument)spine, (InputArgument)(0.075f), (InputArgument)(-0.15f), (InputArgument)(-0.02f), (InputArgument)0f, (InputArgument)(165f), (InputArgument)0f, (InputArgument)true, (InputArgument)true, (InputArgument)false, (InputArgument)true, (InputArgument)2, (InputArgument)true);
                this.on = true;
            }
            else if (!attached)
            {

                if ((!Game.Player.Character.IsClimbing && !Game.Player.Character.IsVaulting) && this.on)
                {
                    this.WeaponOnBack.Delete();
                    this.WeaponOnBack = null;
                    this.on = false;
                    Game.Player.Character.Task.PlayAnimation("mp_arrest_paired", "cop_p1_rf_right_0", 8f, 500, AnimationFlags.UpperBodyOnly | AnimationFlags.AllowRotation);
                }

            }

        }
    }

    public bool IsClimbing(Ped ped)
    {
        bool num;
        if ((Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)ped), (InputArgument)"move_climb", (InputArgument)"bonnet_slide_l_in", (InputArgument)3))
        ||
        (Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)ped), (InputArgument)"move_climb", (InputArgument)"bonnet_slide_l_out", (InputArgument)3))
        ||
        (Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)ped), (InputArgument)"move_climb", (InputArgument)"bonnet_slide_r_in", (InputArgument)3))
        ||
        (Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)ped), (InputArgument)"move_climb", (InputArgument)"bonnet_slide_r_out", (InputArgument)3))
        ||
        (Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)ped), (InputArgument)"move_climb", (InputArgument)"bonnet_slide_l_slide", (InputArgument)3))
        ||
        (Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)ped), (InputArgument)"move_climb", (InputArgument)"bonnet_slide_r_slide", (InputArgument)3))
        ||
        (Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)ped), (InputArgument)"move_climb", (InputArgument)"runclimbup_140_high", (InputArgument)3))
        ||
        (Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)ped), (InputArgument)"move_climb", (InputArgument)"runclimbup_140_high_angled_20", (InputArgument)3))
        ||
        (Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)ped), (InputArgument)"move_climb", (InputArgument)"runclimbup_140_low", (InputArgument)3))
        ||
        (Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)ped), (InputArgument)"move_climb", (InputArgument)"runclimbup_140_low_angled_20", (InputArgument)3))
        ||
        (Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)ped), (InputArgument)"move_climb", (InputArgument)"runclimbup_180_high", (InputArgument)3))
        ||
        (Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)Game.Player.Character), (InputArgument)"move_climb", (InputArgument)"runclimbup_180_high_angled_20", (InputArgument)3))
        ||
        (Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)Game.Player.Character), (InputArgument)"move_climb", (InputArgument)"runclimbup_180_low", (InputArgument)3))
        ||
        (Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)Game.Player.Character), (InputArgument)"move_climb", (InputArgument)"runclimbup_180_low_angled_20", (InputArgument)3))
        ||
        (Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)Game.Player.Character), (InputArgument)"move_climb", (InputArgument)"runclimbup_220", (InputArgument)3))
        ||
         (Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)Game.Player.Character), (InputArgument)"move_climb", (InputArgument)"runclimbup_220_angled_20", (InputArgument)3))
||
(Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)Game.Player.Character), (InputArgument)"move_climb", (InputArgument)"runclimbup_220_high", (InputArgument)3))
||
(Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)Game.Player.Character), (InputArgument)"move_climb", (InputArgument)"runclimbup_220_high_angled_20", (InputArgument)3))
||
(Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)Game.Player.Character), (InputArgument)"move_climb", (InputArgument)"runclimbup_295_low", (InputArgument)3))
||
(Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)Game.Player.Character), (InputArgument)"move_climb", (InputArgument)"runclimbup_295_low_angled_20", (InputArgument)3))
||
(Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)Game.Player.Character), (InputArgument)"move_climb", (InputArgument)"runclimbup_80", (InputArgument)3))
||
(Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)Game.Player.Character), (InputArgument)"move_climb", (InputArgument)"runclimbup_80_angled_20", (InputArgument)3))
||
(Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)Game.Player.Character), (InputArgument)"move_climb", (InputArgument)"standclimbup_140_high", (InputArgument)3))
||
(Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)Game.Player.Character), (InputArgument)"move_climb", (InputArgument)"standclimbup_140_high_angled_20", (InputArgument)3))
||
(Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)Game.Player.Character), (InputArgument)"move_climb", (InputArgument)"standclimbup_140_low", (InputArgument)3))
||
(Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)Game.Player.Character), (InputArgument)"move_climb", (InputArgument)"standclimbup_140_low_angled_20", (InputArgument)3))
||
(Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)Game.Player.Character), (InputArgument)"move_climb", (InputArgument)"standclimbup_180_high", (InputArgument)3))
||
(Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)Game.Player.Character), (InputArgument)"move_climb", (InputArgument)"standclimbup_180_high_angled_20", (InputArgument)3))
||
(Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)Game.Player.Character), (InputArgument)"move_climb", (InputArgument)"standclimbup_180_low", (InputArgument)3))
||
(Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)Game.Player.Character), (InputArgument)"move_climb", (InputArgument)"standclimbup_220", (InputArgument)3))
||
(Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)Game.Player.Character), (InputArgument)"move_climb", (InputArgument)"standclimbup_220_angled_20", (InputArgument)3))
||
(Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)Game.Player.Character), (InputArgument)"move_climb", (InputArgument)"standclimbup_220_high", (InputArgument)3))
||
(Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)Game.Player.Character), (InputArgument)"move_climb", (InputArgument)"standclimbup_220_high_angled_20", (InputArgument)3))
||
(Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)Game.Player.Character), (InputArgument)"move_climb", (InputArgument)"standclimbup_295_low", (InputArgument)3))
||
(Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)Game.Player.Character), (InputArgument)"move_climb", (InputArgument)"standclimbup_295_low_angled_20", (InputArgument)3))
||
(Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)Game.Player.Character), (InputArgument)"move_climb", (InputArgument)"standclimbup_80", (InputArgument)3))
||
(Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)Game.Player.Character), (InputArgument)"move_climb", (InputArgument)"standclimbup_80_angled_20", (InputArgument)3))
||
(Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)Game.Player.Character), (InputArgument)"move_climb", (InputArgument)"walkclimbup_140_low", (InputArgument)3))
||
(Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, (InputArgument)((Entity)Game.Player.Character), (InputArgument)"move_climb", (InputArgument)"walkclimbup_80", (InputArgument)3)))
        {


            num = true;
            return num;
        }
        else
            return false;



    }

}
