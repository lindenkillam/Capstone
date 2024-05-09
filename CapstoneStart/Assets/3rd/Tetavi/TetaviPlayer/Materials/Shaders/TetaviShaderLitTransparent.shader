Shader "Tetavi/TetaviShaderLitTransparent"
{
    Properties
    {
		[HideInInspector]_MainTex("Albedo, Metallic", 2D) = "white" {}
		[HideInInspector]_MS("Material segmentation", Float) = 0
		[HideInInspector]_NM("Normal maps", Float) = 1
		[HideInInspector][Toggle(SHOW_GLOBAL)]
		Expend_1_Global("Global", Float) = 0
		[Toggle] _Optimized("Optimized", Float) = 0
		Sample_radius("Sample Radius", range(0.0, 4)) = 0
		Normal_Map_Intensity("Normal Map Intensity", Float) = 1
		[HideInInspector] _Expend_Stop("", Float) = 0
			
		[HideInInspector][Toggle(SHOW)] _Expend_0_Clothes("Clothes", Float) = 0
		////////////// Clothes Drop Down Open

			//Hat
			[HideInInspector][Toggle(SHOW)] _Expend_1_Hat("hat", Float) = 0
				[Toggle] Hide_hat("Hide", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_2_Hat("Color Correction", Float) = 0
					Color_hat("Color", Color) = (1, 1, 1, 1)
					Hue_hat("Hue hat",range(-180, 180)) = 0
					Saturation_hat("Saturation hat",range(-100, 100)) = 0
					Value_hat("Value hat",range(-100, 100)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0

				[HideInInspector][Toggle(SHOW)] _Expend_3_Hat("PBR", Float) = 0
					MetallicBias_hat("Metallic Bias", Range(0, 1)) = 0
					MetallicScale_hat("Metallic Scale", Range(0, 2)) = 0
					SmoothnessBias_hat("Smoothness Bias", Range(0, 1)) = 0
					SmoothnessScale_hat("Smoothness Scale", Range(0, 2)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
			[HideInInspector] _Expend_Stop("", Float) = 0

			//gloves
			[HideInInspector][Toggle(SHOW)] _Expend_1_Gloves("gloves", Float) = 0
				[Toggle] Hide_gloves("Hide", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_2_Gloves("Color Correction", Float) = 0
					Color_gloves("Color", Color) = (1, 1, 1, 1)
					Hue_gloves("Hue gloves",range(-180, 180)) = 0
					Saturation_gloves("Saturation gloves",range(-100, 100)) = 0
					Value_gloves("Value gloves",range(-100, 100)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_3_Gloves("PBR", Float) = 0
					MetallicBias_gloves("Metallic Bias", Range(0, 1)) = 0
					MetallicScale_gloves("Metallic Scale", Range(0, 2)) = 0
					SmoothnessBias_gloves("Smoothness Bias", Range(0, 1)) = 0
					SmoothnessScale_gloves("Smoothness Scale", Range(0, 2)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
			[HideInInspector] _Expend_Stop("", Float) = 0

			//glasses
			[HideInInspector][Toggle(SHOW)] _Expend_1_Glasses("glasses", Float) = 0
				[Toggle] Hide_glasses("Hide", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_2_Glasses("Color Correction", Float) = 0
					Color_glasses("Color", Color) = (1, 1, 1, 1)
					Hue_glasses("Hue glasses",range(-180, 180)) = 0
					Saturation_glasses("Saturation glasses",range(-100, 100)) = 0
					Value_glasses("Value glasses",range(-100, 100)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_3_Glasses("PBR", Float) = 0
					MetallicBias_glasses("Metallic Bias", Range(0, 1)) = 0
					MetallicScale_glasses("Metallic Scale", Range(0, 2)) = 0
					SmoothnessBias_glasses("Smoothness Bias", Range(0, 1)) = 0
					SmoothnessScale_glasses("Smoothness Scale", Range(0, 2)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
			[HideInInspector] _Expend_Stop("", Float) = 0

			//upper_clothes
			[HideInInspector][Toggle(SHOW)] _Expend_1_Upper_Clothes("upper_clothes", Float) = 0
				[Toggle] Hide_upper_clothes("Hide", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_2_Upper_Clothes("Color Correction", Float) = 0
					Color_upper_clothes("Color", Color) = (1, 1, 1, 1)
					Hue_upper_clothes("Hue upper_clothes",range(-180, 180)) = 0
					Saturation_upper_clothes("Saturation upper_clothes",range(-100, 100)) = 0
					Value_upper_clothes("Value upper_clothes",range(-100, 100)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_3_Upper_Clothes("PBR", Float) = 0
					MetallicBias_upper_clothes("Metallic Bias", Range(0, 1)) = 0
					MetallicScale_upper_clothes("Metallic Scale", Range(0, 2)) = 0
					SmoothnessBias_upper_clothes("Smoothness Bias", Range(0, 1)) = 0
					SmoothnessScale_upper_clothes("Smoothness Scale", Range(0, 2)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
			[HideInInspector] _Expend_Stop("", Float) = 0

			//dress
			[HideInInspector][Toggle(SHOW)] _Expend_1_Dress("dress", Float) = 0
				[Toggle] Hide_dress("Hide", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_2_Dress("Color Correction", Float) = 0
					Color_dress("Color", Color) = (1, 1, 1, 1)
					Hue_dress("Hue dress",range(-180, 180)) = 0
					Saturation_dress("Saturation dress",range(-100, 100)) = 0
					Value_dress("Value dress",range(-100, 100)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_3_Dress("PBR", Float) = 0
					MetallicBias_dress("Metallic Bias", Range(0, 1)) = 0
					MetallicScale_dress("Metallic Scale", Range(0, 2)) = 0
					SmoothnessBias_dress("Smoothness Bias", Range(0, 1)) = 0
					SmoothnessScale_dress("Smoothness Scale", Range(0, 2)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
			[HideInInspector] _Expend_Stop("", Float) = 0

			//coat
			[HideInInspector][Toggle(SHOW)] _Expend_1_Coat("coat", Float) = 0
				[Toggle] Hide_coat("Hide", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_2_Coat("Color Correction", Float) = 0
					Color_coat("Color", Color) = (1, 1, 1, 1)
					Hue_coat("Hue coat",range(-180, 180)) = 0
					Saturation_coat("Saturation coat",range(-100, 100)) = 0
					Value_coat("Value coat",range(-100, 100)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_3_Coat("PBR", Float) = 0
					MetallicBias_coat("Metallic Bias", Range(0, 1)) = 0
					MetallicScale_coat("Metallic Scale", Range(0, 2)) = 0
					SmoothnessBias_coat("Smoothness Bias", Range(0, 1)) = 0
					SmoothnessScale_coat("Smoothness Scale", Range(0, 2)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
			[HideInInspector] _Expend_Stop("", Float) = 0

			//socks
			[HideInInspector][Toggle(SHOW)] _Expend_1_Socks("socks", Float) = 0
				[Toggle] Hide_socks("Hide", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_2_Socks("Color Correction", Float) = 0
					Color_socks("Color", Color) = (1, 1, 1, 1)
					Hue_socks("Hue socks",range(-180, 180)) = 0
					Saturation_socks("Saturation socks",range(-100, 100)) = 0
					Value_socks("Value socks",range(-100, 100)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_3_Socks("PBR", Float) = 0
					MetallicBias_socks("Metallic Bias", Range(0, 1)) = 0
					MetallicScale_socks("Metallic Scale", Range(0, 2)) = 0
					SmoothnessBias_socks("Smoothness Bias", Range(0, 1)) = 0
					SmoothnessScale_socks("Smoothness Scale", Range(0, 2)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
			[HideInInspector] _Expend_Stop("", Float) = 0

			//pants
			[HideInInspector][Toggle(SHOW)] _Expend_1_Pants("pants", Float) = 0
				[Toggle] Hide_pants("Hide", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_2_Pants("Color Correction", Float) = 0
					Color_pants("Color", Color) = (1, 1, 1, 1)
					Hue_pants("Hue pants",range(-180, 180)) = 0
					Saturation_pants("Saturation pants",range(-100, 100)) = 0
					Value_pants("Value pants",range(-100, 100)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_3_Pants("PBR", Float) = 0
					MetallicBias_pants("Metallic Bias", Range(0, 1)) = 0
					MetallicScale_pants("Metallic Scale", Range(0, 2)) = 0
					SmoothnessBias_pants("Smoothness Bias", Range(0, 1)) = 0
					SmoothnessScale_pants("Smoothness Scale", Range(0, 2)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
			[HideInInspector] _Expend_Stop("", Float) = 0

			//scarf
			[HideInInspector][Toggle(SHOW)] _Expend_1_Scarf("scarf", Float) = 0
				[Toggle] Hide_scarf("Hide", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_2_Scarf("Color Correction", Float) = 0
					Color_scarf("Color", Color) = (1, 1, 1, 1)
					Hue_scarf("Hue scarf",range(-180, 180)) = 0
					Saturation_scarf("Saturation scarf",range(-100, 100)) = 0
					Value_scarf("Value scarf",range(-100, 100)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_3_Scarf("PBR", Float) = 0
					MetallicBias_scarf("Metallic Bias", Range(0, 1)) = 0
					MetallicScale_scarf("Metallic Scale", Range(0, 2)) = 0
					SmoothnessBias_scarf("Smoothness Bias", Range(0, 1)) = 0
					SmoothnessScale_scarf("Smoothness Scale", Range(0, 2)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
			[HideInInspector] _Expend_Stop("", Float) = 0

			//skirt
			[HideInInspector][Toggle(SHOW)] _Expend_1_Skirt("skirt", Float) = 0
				[Toggle] Hide_skirt("Hide", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_2_Skirt("Color Correction", Float) = 0
					Color_skirt("Color", Color) = (1, 1, 1, 1)
					Hue_skirt("Hue skirt",range(-180, 180)) = 0
					Saturation_skirt("Saturation skirt",range(-100, 100)) = 0
					Value_skirt("Value skirt",range(-100, 100)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_3_Skirt("PBR", Float) = 0
					MetallicBias_skirt("Metallic Bias", Range(0, 1)) = 0
					MetallicScale_skirt("Metallic Scale", Range(0, 2)) = 0
					SmoothnessBias_skirt("Smoothness Bias", Range(0, 1)) = 0
					SmoothnessScale_skirt("Smoothness Scale", Range(0, 2)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
			[HideInInspector] _Expend_Stop("", Float) = 0

			//left_shoe
			[HideInInspector][Toggle(SHOW)] _Expend_1_Left_Shoe("left_shoe", Float) = 0
				[Toggle] Hide_left_shoe("Hide", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_2_Left_Shoe("Color Correction", Float) = 0
					Color_left_shoe("Color", Color) = (1, 1, 1, 1)
					Hue_left_shoe("Hue left_shoe",range(-180, 180)) = 0
					Saturation_left_shoe("Saturation left_shoe",range(-100, 100)) = 0
					Value_left_shoe("Value left_shoe",range(-100, 100)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_3_Left_Shoe("PBR", Float) = 0
					MetallicBias_left_shoe("Metallic Bias", Range(0, 1)) = 0
					MetallicScale_left_shoe("Metallic Scale", Range(0, 2)) = 0
					SmoothnessBias_left_shoe("Smoothness Bias", Range(0, 1)) = 0
					SmoothnessScale_left_shoe("Smoothness Scale", Range(0, 2)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
			[HideInInspector] _Expend_Stop("", Float) = 0

			//right_shoe
			[HideInInspector][Toggle(SHOW)] _Expend_1_Right_Shoe("right_shoe", Float) = 0
				[Toggle] Hide_right_shoe("Hide", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_2_Right_Shoe("Color Correction", Float) = 0
					Color_right_shoe("Color", Color) = (1, 1, 1, 1)
					Hue_right_shoe("Hue right_shoe",range(-180, 180)) = 0
					Saturation_right_shoe("Saturation right_shoe",range(-100, 100)) = 0
					Value_right_shoe("Value right_shoe",range(-100, 100)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_3_Right_Shoe("PBR", Float) = 0
					MetallicBias_right_shoe("Metallic Bias", Range(0, 1)) = 0
					MetallicScale_right_shoe("Metallic Scale", Range(0, 2)) = 0
					SmoothnessBias_right_shoe("Smoothness Bias", Range(0, 1)) = 0
					SmoothnessScale_right_shoe("Smoothness Scale", Range(0, 2)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
			[HideInInspector] _Expend_Stop("", Float) = 0


	////////////// Clothes Drop Down Close
	[HideInInspector] _Expend_Stop("", Float) = 0

	////////////// Body Drop Down Open
	[HideInInspector][Toggle(SHOW)] _Expend_0_Skin("Body", Float) = 0

			//hair
			[HideInInspector][Toggle(SHOW)] _Expend_1_Hair("hair", Float) = 0
				[Toggle] Hide_hair("Hide", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_2_Hair("Color Correction", Float) = 0
					Color_hair("Color", Color) = (1, 1, 1, 1)
					Hue_hair("Hue hair",range(-180, 180)) = 0
					Saturation_hair("Saturation hair",range(-100, 100)) = 0
					Value_hair("Value hair",range(-100, 100)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0

				[HideInInspector][Toggle(SHOW)] _Expend_3_Hair("PBR", Float) = 0
					MetallicBias_hair("Metallic Bias", Range(0, 1)) = 0
					MetallicScale_hair("Metallic Scale", Range(0, 2)) = 0
					SmoothnessBias_hair("Smoothness Bias", Range(0, 1)) = 0
					SmoothnessScale_hair("Smoothness Scale", Range(0, 2)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
			[HideInInspector] _Expend_Stop("", Float) = 0

			//torso_skin
			[HideInInspector][Toggle(SHOW)] _Expend_1_Torso_Skin("torso_skin", Float) = 0
				[Toggle] Hide_torso_skin("Hide", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_2_Torso_Skin("Color Correction", Float) = 0
					Color_torso_skin("Color", Color) = (1, 1, 1, 1)
					Hue_torso_skin("Hue torso_skin",range(-180, 180)) = 0
					Saturation_torso_skin("Saturation torso_skin",range(-100, 100)) = 0
					Value_torso_skin("Value torso_skin",range(-100, 100)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_3_Torso_Skin("PBR", Float) = 0
					MetallicBias_torso_skin("Metallic Bias", Range(0, 1)) = 0
					MetallicScale_torso_skin("Metallic Scale", Range(0, 2)) = 0
					SmoothnessBias_torso_skin("Smoothness Bias", Range(0, 1)) = 0
					SmoothnessScale_torso_skin("Smoothness Scale", Range(0, 2)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
			[HideInInspector] _Expend_Stop("", Float) = 0


			//face
			[HideInInspector][Toggle(SHOW)] _Expend_1_Face("face", Float) = 0
				[Toggle] Hide_face("Hide", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_2_Face("Color Correction", Float) = 0
					Color_face("Color", Color) = (1, 1, 1, 1)
					Hue_face("Hue face",range(-180, 180)) = 0
					Saturation_face("Saturation face",range(-100, 100)) = 0
					Value_face("Value face",range(-100, 100)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_3_Face("PBR", Float) = 0
					MetallicBias_face("Metallic Bias", Range(0, 1)) = 0
					MetallicScale_face("Metallic Scale", Range(0, 2)) = 0
					SmoothnessBias_face("Smoothness Bias", Range(0, 1)) = 0
					SmoothnessScale_face("Smoothness Scale", Range(0, 2)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
			[HideInInspector] _Expend_Stop("", Float) = 0

			//left_arm
			[HideInInspector][Toggle(SHOW)] _Expend_1_Left_Arm("left_arm", Float) = 0
				[Toggle] Hide_left_arm("Hide", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_2_Left_Arm("Color Correction", Float) = 0
					Color_left_arm("Color", Color) = (1, 1, 1, 1)
					Hue_left_arm("Hue left_arm",range(-180, 180)) = 0
					Saturation_left_arm("Saturation left_arm",range(-100, 100)) = 0
					Value_left_arm("Value left_arm",range(-100, 100)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_3_Left_Arm("PBR", Float) = 0
					MetallicBias_left_arm("Metallic Bias", Range(0, 1)) = 0
					MetallicScale_left_arm("Metallic Scale", Range(0, 2)) = 0
					SmoothnessBias_left_arm("Smoothness Bias", Range(0, 1)) = 0
					SmoothnessScale_left_arm("Smoothness Scale", Range(0, 2)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
			[HideInInspector] _Expend_Stop("", Float) = 0

			//right_arm
			[HideInInspector][Toggle(SHOW)] _Expend_1_Right_Arm("right_arm", Float) = 0
				[Toggle] Hide_right_arm("Hide", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_2_Right_Arm("Color Correction", Float) = 0
					Color_right_arm("Color", Color) = (1, 1, 1, 1)
					Hue_right_arm("Hue right_arm",range(-180, 180)) = 0
					Saturation_right_arm("Saturation right_arm",range(-100, 100)) = 0
					Value_right_arm("Value right_arm",range(-100, 100)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_3_Right_Arm("PBR", Float) = 0
					MetallicBias_right_arm("Metallic Bias", Range(0, 1)) = 0
					MetallicScale_right_arm("Metallic Scale", Range(0, 2)) = 0
					SmoothnessBias_right_arm("Smoothness Bias", Range(0, 1)) = 0
					SmoothnessScale_right_arm("Smoothness Scale", Range(0, 2)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
			[HideInInspector] _Expend_Stop("", Float) = 0

			//left_leg
			[HideInInspector][Toggle(SHOW)] _Expend_1_Left_Leg("left_leg", Float) = 0
				[Toggle] Hide_left_leg("Hide", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_2_Left_Leg("Color Correction", Float) = 0
					Color_left_leg("Color", Color) = (1, 1, 1, 1)
					Hue_left_leg("Hue left_leg",range(-180, 180)) = 0
					Saturation_left_leg("Saturation left_leg",range(-100, 100)) = 0
					Value_left_leg("Value left_leg",range(-100, 100)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_3_Left_Leg("PBR", Float) = 0
					MetallicBias_left_leg("Metallic Bias", Range(0, 1)) = 0
					MetallicScale_left_leg("Metallic Scale", Range(0, 2)) = 0
					SmoothnessBias_left_leg("Smoothness Bias", Range(0, 1)) = 0
					SmoothnessScale_left_leg("Smoothness Scale", Range(0, 2)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
			[HideInInspector] _Expend_Stop("", Float) = 0

			//right_leg
			[HideInInspector][Toggle(SHOW)] _Expend_1_Right_Leg("right_leg", Float) = 0
				[Toggle] Hide_right_leg("Hide", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_2_Right_Leg("Color Correction", Float) = 0
					Color_right_leg("Color", Color) = (1, 1, 1, 1)
					Hue_right_leg("Hue right_leg",range(-180, 180)) = 0
					Saturation_right_leg("Saturation right_leg",range(-100, 100)) = 0
					Value_right_leg("Value right_leg",range(-100, 100)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_3_Right_Leg("PBR", Float) = 0
					MetallicBias_right_leg("Metallic Bias", Range(0, 1)) = 0
					MetallicScale_right_leg("Metallic Scale", Range(0, 2)) = 0
					SmoothnessBias_right_leg("Smoothness Bias", Range(0, 1)) = 0
					SmoothnessScale_right_leg("Smoothness Scale", Range(0, 2)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
			[HideInInspector] _Expend_Stop("", Float) = 0

		

			//mouth
			[HideInInspector][Toggle(SHOW)] _Expend_1_Mouth("mouth", Float) = 0
				[Toggle] Hide_mouth("Hide", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_2_Mouth("Color Correction", Float) = 0
					Color_mouth("Color", Color) = (1, 1, 1, 1)
					Hue_mouth("Hue mouth",range(-180, 180)) = 0
					Saturation_mouth("Saturation mouth",range(-100, 100)) = 0
					Value_mouth("Value mouth",range(-100, 100)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_3_Mouth("PBR", Float) = 0
					MetallicBias_mouth("Metallic Bias", Range(0, 1)) = 0
					MetallicScale_mouth("Metallic Scale", Range(0, 2)) = 0
					SmoothnessBias_mouth("Smoothness Bias", Range(0, 1)) = 0
					SmoothnessScale_mouth("Smoothness Scale", Range(0, 2)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
			[HideInInspector] _Expend_Stop("", Float) = 0

			//left_eye
			[HideInInspector][Toggle(SHOW)] _Expend_1_Left_Eye("left_eye", Float) = 0
				[Toggle] Hide_left_eye("Hide", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_2_Left_Eye("Color Correction", Float) = 0
					Color_left_eye("Color", Color) = (1, 1, 1, 1)
					Hue_left_eye("Hue left_eye",range(-180, 180)) = 0
					Saturation_left_eye("Saturation left_eye",range(-100, 100)) = 0
					Value_left_eye("Value left_eye",range(-100, 100)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_3_Left_Eye("PBR", Float) = 0
					MetallicBias_left_eye("Metallic Bias", Range(0, 1)) = 0
					MetallicScale_left_eye("Metallic Scale", Range(0, 2)) = 0
					SmoothnessBias_left_eye("Smoothness Bias", Range(0, 1)) = 0
					SmoothnessScale_left_eye("Smoothness Scale", Range(0, 2)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
			[HideInInspector] _Expend_Stop("", Float) = 0

			//right_eye
			[HideInInspector][Toggle(SHOW)] _Expend_1_Right_Eye("right_eye", Float) = 0
				[Toggle] Hide_right_eye("Hide", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_2_Right_Eye("Color Correction", Float) = 0
					Color_right_eye("Color", Color) = (1, 1, 1, 1)
					Hue_right_eye("Hue right_eye",range(-180, 180)) = 0
					Saturation_right_eye("Saturation right_eye",range(-100, 100)) = 0
					Value_right_eye("Value right_eye",range(-100, 100)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_3_Right_Eye("PBR", Float) = 0
					MetallicBias_right_eye("Metallic Bias", Range(0, 1)) = 0
					MetallicScale_right_eye("Metallic Scale", Range(0, 2)) = 0
					SmoothnessBias_right_eye("Smoothness Bias", Range(0, 1)) = 0
					SmoothnessScale_right_eye("Smoothness Scale", Range(0, 2)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
			[HideInInspector] _Expend_Stop("", Float) = 0


	////////////// Body Drop Down Close
	[HideInInspector] _Expend_Stop("", Float) = 0

	////////////// Unknown Drop Down Open
	[HideInInspector][Toggle(SHOW)] _Expend_0_Unknown("Unknown", Float) = 0

			//unknown1
			[HideInInspector][Toggle(SHOW)] _Expend_1_Unknown1("unknown1", Float) = 0
				[Toggle] Hide_unknown1("Hide", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_2_Unknown1("Color Correction", Float) = 0
					Color_unknown1("Color", Color) = (1, 1, 1, 1)
					Hue_unknown1("Hue unknown1",range(-180, 180)) = 0
					Saturation_unknown1("Saturation unknown1",range(-100, 100)) = 0
					Value_unknown1("Value unknown1",range(-100, 100)) = 0

				[HideInInspector] _Expend_Stop("", Float) = 0

				[HideInInspector][Toggle(SHOW)] _Expend_3_Unknown1("PBR", Float) = 0
					MetallicBias_unknown1("Metallic Bias", Range(0, 1)) = 0
					MetallicScale_unknown1("Metallic Scale", Range(0, 2)) = 0
					SmoothnessBias_unknown1("Smoothness Bias", Range(0, 1)) = 0
					SmoothnessScale_unknown1("Smoothness Scale", Range(0, 2)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
			[HideInInspector] _Expend_Stop("", Float) = 0

			//unknown2
			[HideInInspector][Toggle(SHOW)] _Expend_1_Unknown2("unknown2", Float) = 0
				[Toggle] Hide_unknown2("Hide", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_2_Unknown2("Color Correction", Float) = 0
					Color_unknown2("Color", Color) = (1, 1, 1, 1)
					Hue_unknown2("Hue unknown2",range(-180, 180)) = 0
					Saturation_unknown2("Saturation unknown2",range(-100, 100)) = 0
					Value_unknown2("Value unknown2",range(-100, 100)) = 0

				[HideInInspector] _Expend_Stop("", Float) = 0

				[HideInInspector][Toggle(SHOW)] _Expend_3_Unknown2("PBR", Float) = 0
					MetallicBias_unknown2("Metallic Bias", Range(0, 1)) = 0
					MetallicScale_unknown2("Metallic Scale", Range(0, 2)) = 0
					SmoothnessBias_unknown2("Smoothness Bias", Range(0, 1)) = 0
					SmoothnessScale_unknown2("Smoothness Scale", Range(0, 2)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
			[HideInInspector] _Expend_Stop("", Float) = 0

			//unknown3
			[HideInInspector][Toggle(SHOW)] _Expend_1_Unknown3("unknown3", Float) = 0
				[Toggle] Hide_unknown3("Hide", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_2_Unknown3("Color Correction", Float) = 0
					Color_unknown3("Color", Color) = (1, 1, 1, 1)
					Hue_unknown3("Hue unknown3",range(-180, 180)) = 0
					Saturation_unknown3("Saturation unknown3",range(-100, 100)) = 0
					Value_unknown3("Value unknown3",range(-100, 100)) = 0

				[HideInInspector] _Expend_Stop("", Float) = 0

				[HideInInspector][Toggle(SHOW)] _Expend_3_Unknown3("PBR", Float) = 0
					MetallicBias_unknown3("Metallic Bias", Range(0, 1)) = 0
					MetallicScale_unknown3("Metallic Scale", Range(0, 2)) = 0
					SmoothnessBias_unknown3("Smoothness Bias", Range(0, 1)) = 0
					SmoothnessScale_unknown3("Smoothness Scale", Range(0, 2)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
			[HideInInspector] _Expend_Stop("", Float) = 0

			//unknown4
			[HideInInspector][Toggle(SHOW)] _Expend_1_Unknown4("unknown4", Float) = 0
			[Toggle] Hide_unknown4("Hide", Float) = 0
				[HideInInspector][Toggle(SHOW)] _Expend_2_Unknown4("Color Correction", Float) = 0
					Color_unknown4("Color", Color) = (1, 1, 1, 1)
					Hue_unknown4("Hue unknown4",range(-180, 180)) = 0
					Saturation_unknown4("Saturation unknown4",range(-100, 100)) = 0
					Value_unknown4("Value unknown4",range(-100, 100)) = 0

				[HideInInspector] _Expend_Stop("", Float) = 0

				[HideInInspector][Toggle(SHOW)] _Expend_3_Unknown4("PBR", Float) = 0
					MetallicBias_unknown4("Metallic Bias", Range(0, 1)) = 0
					MetallicScale_unknown4("Metallic Scale", Range(0, 2)) = 0
					SmoothnessBias_unknown4("Smoothness Bias", Range(0, 1)) = 0
					SmoothnessScale_unknown4("Smoothness Scale", Range(0, 2)) = 0
				[HideInInspector] _Expend_Stop("", Float) = 0
			[HideInInspector] _Expend_Stop("", Float) = 0

	////////////// Unknown Drop Down Close
	[HideInInspector] _Expend_Stop("", Float) = 0
			

		


}



    SubShader {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        
        CGPROGRAM
        #pragma shader_feature _MATERIAL_MASKED
        #pragma surface surf Standard
        #pragma target 3.0
		
        // #include "PBRLib.cginc"

        sampler2D _TexY;
		float4 _TexY_TexelSize;
		sampler2D _TexUV;
		sampler2D _TexNM;
		half _MS;
		half _NM;
		half _Optimized;
		uniform float Sample_radius;
		float Normal_Map_Intensity;
		

		float4 tet_sample(float2 uv, float offset_x, float offset_y) {
			uv.x += offset_x;
			uv.y += offset_y;
			float y = tex2D(_TexY, uv).r;
			float2 UV_rg = tex2D(_TexUV, uv);
			float u = (UV_rg.r * 0.872 - 0.436);
			float v = (UV_rg.g * 1.230 - 0.615);

			float3 rgb;
			
			rgb.r = clamp(y + 1.13983 * v, 0.0, 1.0);
			rgb.g = clamp(y - 0.39465 * u - 0.58060 * v, 0.0, 1.0);
			rgb.b = clamp(y + 2.03211 * u, 0.0, 1.0);

			float4 col = float4(rgb, 1);

			return col*2;

		}

		int tet_label(float2 uv, float offset_x, float offset_y) {
			uv.x += offset_x;
			uv.y += offset_y;
			float2 label = uv;

			label.g = label.g - 0.5;
			float yL = tex2Dlod(_TexY,  float4(label, 0, 0)).r;
			float2 LAbelUV_rg = tex2Dlod(_TexUV, float4(label, 0, 0));
			float uL = LAbelUV_rg.r;
			float vL = LAbelUV_rg.g;

			static const int s[] = { 0,0,0,0,1,2,2,2,2 };


			int Label = 9 * s[int((yL * 255 + 16) / 32)] + 3 * s[int((uL * 255 + 16) / 32)] + s[int((vL * 255 + 16) / 32)];

			return Label;

		}



		float Epsilon = 1e-10;

		float3 RGBtoHCV(in float3 RGB)
		{
			// Based on work by Sam Hocevar and Emil Persson
			float4 P = (RGB.g < RGB.b) ? float4(RGB.bg, -1.0, 2.0 / 3.0) : float4(RGB.gb, 0.0, -1.0 / 3.0);
			float4 Q = (RGB.r < P.x) ? float4(P.xyw, RGB.r) : float4(RGB.r, P.yzx);
			float C = Q.x - min(Q.w, Q.y);
			float H = abs((Q.w - Q.y) / (6 * C + Epsilon) + Q.z);
			return float3(H, C, Q.x);
		}

		float3 RGBtoHSV(in float3 RGB)
		{
			float3 HCV = RGBtoHCV(RGB);
			float S = HCV.y / (HCV.z + Epsilon);
			return float3(HCV.x, S, HCV.z);
		}

		float3 HUEtoRGB(in float H)
		{
			float R = abs(H * 6 - 3) - 1;
			float G = 2 - abs(H * 6 - 2);
			float B = 2 - abs(H * 6 - 4);
			return saturate(float3(R, G, B));
		}

		float3 HSVtoRGB(in float3 HSV)
		{
			float3 RGB = HUEtoRGB(HSV.x);
			return ((RGB - 1) * HSV.y + 1) * HSV.z;
		}

		float mod(float x, float y)
		{
			return x - y * floor(x / y);
		}

		float3 hsv(float3 rgb,float3 hue,float3 saturation,float3 value)
		{
			float3 hsv = RGBtoHSV(rgb);
			hsv.x = mod((hsv.x + (hue) / 360), 1);
			hsv.y = clamp(hsv.y + saturation / 100, 0, 1);
			hsv.z = (hsv.z + value / 50);
			return HSVtoRGB(hsv);
		}
		
        
		// hat
		half4 Color_hat;
		half Hide_hat;
		half Hue_hat;
		half Saturation_hat;
		half Value_hat;

		half MetallicBias_hat;
		half MetallicScale_hat;
		half SmoothnessBias_hat;
		half SmoothnessScale_hat;

		// hair
		half4 Color_hair;
		half Hide_hair;
		half Hue_hair;
		half Saturation_hair;
		half Value_hair;

		half MetallicBias_hair;
		half MetallicScale_hair;
		half SmoothnessBias_hair;
		half SmoothnessScale_hair;

		// gloves
		half Hide_gloves;
		half Hue_gloves;
		half Saturation_gloves;
		half Value_gloves;

		half4 Color_gloves;
		half MetallicBias_gloves;
		half MetallicScale_gloves;
		half SmoothnessBias_gloves;
		half SmoothnessScale_gloves;

		// glasses
		half Hide_glasses;
		half Hue_glasses;
		half Saturation_glasses;
		half Value_glasses;

		half4 Color_glasses;
		half MetallicBias_glasses;
		half MetallicScale_glasses;
		half SmoothnessBias_glasses;
		half SmoothnessScale_glasses;

		// dress
		half4 Color_dress;
		half Hide_dress;
		half Hue_dress;
		half Saturation_dress;
		half Value_dress;

		half MetallicBias_dress;
		half MetallicScale_dress;
		half SmoothnessBias_dress;
		half SmoothnessScale_dress;

		// coat
		half4 Color_coat;
		half Hide_coat;
		half Hue_coat;
		half Saturation_coat;
		half Value_coat;

		half MetallicBias_coat;
		half MetallicScale_coat;
		half SmoothnessBias_coat;
		half SmoothnessScale_coat;

		// pants
		half4 Color_pants;
		half Hide_pants;
		half Hue_pants;
		half Saturation_pants;
		half Value_pants;

		half MetallicBias_pants;
		half MetallicScale_pants;
		half SmoothnessBias_pants;
		half SmoothnessScale_pants;

		// scarf
		half4 Color_scarf;
		half Hide_scarf;
		half Hue_scarf;
		half Saturation_scarf;
		half Value_scarf;

		half MetallicBias_scarf;
		half MetallicScale_scarf;
		half SmoothnessBias_scarf;
		half SmoothnessScale_scarf;

		// skirt
		half4 Color_skirt;
		half Hide_skirt;
		half Hue_skirt;
		half Saturation_skirt;
		half Value_skirt;

		half MetallicBias_skirt;
		half MetallicScale_skirt;
		half SmoothnessBias_skirt;
		half SmoothnessScale_skirt;

		// right_arm
		half4 Color_right_arm;
		half Hide_right_arm;
		half Hue_right_arm;
		half Saturation_right_arm;
		half Value_right_arm;

		half MetallicBias_right_arm;
		half MetallicScale_right_arm;
		half SmoothnessBias_right_arm;
		half SmoothnessScale_right_arm;

		// right_leg
		half4 Color_right_leg;
		half Hide_right_leg;
		half Hue_right_leg;
		half Saturation_right_leg;
		half Value_right_leg;

		half MetallicBias_right_leg;
		half MetallicScale_right_leg;
		half SmoothnessBias_right_leg;
		half SmoothnessScale_right_leg;

		// right_shoe
		half4 Color_right_shoe;
		half Hide_right_shoe;
		half Hue_right_shoe;
		half Saturation_right_shoe;
		half Value_right_shoe;

		half MetallicBias_right_shoe;
		half MetallicScale_right_shoe;
		half SmoothnessBias_right_shoe;
		half SmoothnessScale_right_shoe;

		// left_shoe
		half4 Color_left_shoe;
		half Hide_left_shoe;
		half Hue_left_shoe;
		half Saturation_left_shoe;
		half Value_left_shoe;

		half MetallicBias_left_shoe;
		half MetallicScale_left_shoe;
		half SmoothnessBias_left_shoe;
		half SmoothnessScale_left_shoe;

		// left_leg
		half4 Color_left_leg;
		half Hide_left_leg;
		half Hue_left_leg;
		half Saturation_left_leg;
		half Value_left_leg;

		half MetallicBias_left_leg;
		half MetallicScale_left_leg;
		half SmoothnessBias_left_leg;
		half SmoothnessScale_left_leg;

		// left_arm
		half4 Color_left_arm;
		half Hide_left_arm;
		half Hue_left_arm;
		half Saturation_left_arm;
		half Value_left_arm;

		half MetallicBias_left_arm;
		half MetallicScale_left_arm;
		half SmoothnessBias_left_arm;
		half SmoothnessScale_left_arm;

		// upper_clothes
		half4 Color_upper_clothes;
		half Hide_upper_clothes;
		half Hue_upper_clothes;
		half Saturation_upper_clothes;
		half Value_upper_clothes;

		half MetallicBias_upper_clothes;
		half MetallicScale_upper_clothes;
		half SmoothnessBias_upper_clothes;
		half SmoothnessScale_upper_clothes;


		// face
		half4 Color_face;
		half Hide_face;
		half Hue_face;
		half Saturation_face;
		half Value_face;

		half MetallicBias_face;
		half MetallicScale_face;
		half SmoothnessBias_face;
		half SmoothnessScale_face;


		// torso_skin
		half4 Color_torso_skin;
		half Hide_torso_skin;
		half Hue_torso_skin;
		half Saturation_torso_skin;
		half Value_torso_skin;

		half MetallicBias_torso_skin;
		half MetallicScale_torso_skin;
		half SmoothnessBias_torso_skin;
		half SmoothnessScale_torso_skin;


		// mouth
		half4 Color_mouth;
		half Hide_mouth;
		half Hue_mouth;
		half Saturation_mouth;
		half Value_mouth;

		half MetallicBias_mouth;
		half MetallicScale_mouth;
		half SmoothnessBias_mouth;
		half SmoothnessScale_mouth;

		// left_eye
		half4 Color_left_eye;
		half Hide_left_eye;
		half Hue_left_eye;
		half Saturation_left_eye;
		half Value_left_eye;

		half MetallicBias_left_eye;
		half MetallicScale_left_eye;
		half SmoothnessBias_left_eye;
		half SmoothnessScale_left_eye;

		// right_eye
		half4 Color_right_eye;
		half Hide_right_eye;
		half Hue_right_eye;
		half Saturation_right_eye;
		half Value_right_eye;

		half MetallicBias_right_eye;
		half MetallicScale_right_eye;
		half SmoothnessBias_right_eye;
		half SmoothnessScale_right_eye;


		// socks
		half4 Color_socks;
		half Hide_socks;
		half Hue_socks;
		half Saturation_socks;
		half Value_socks;

		half MetallicBias_socks;
		half MetallicScale_socks;
		half SmoothnessBias_socks;
		half SmoothnessScale_socks;



		// unknown 1
		half4 Color_unknown1;
		half Hide_unknown1;
		half Hue_unknown1;
		half Saturation_unknown1;
		half Value_unknown1;

		half MetallicBias_unknown1;
		half MetallicScale_unknown1;
		half SmoothnessBias_unknown1;
		half SmoothnessScale_unknown1;

		// unknown 1
		half4 Color_unknown2;
		half Hide_unknown2;
		half Hue_unknown2;
		half Saturation_unknown2;
		half Value_unknown2;

		half MetallicBias_unknown2;
		half MetallicScale_unknown2;
		half SmoothnessBias_unknown2;
		half SmoothnessScale_unknown2;

		// unknown 1
		half4 Color_unknown3;
		half Hide_unknown3;
		half Hue_unknown3;
		half Saturation_unknown3;
		half Value_unknown3;

		half MetallicBias_unknown3;
		half MetallicScale_unknown3;
		half SmoothnessBias_unknown3;
		half SmoothnessScale_unknown3;

		// unknown 1
		half4 Color_unknown4;
		half Hide_unknown4;
		half Hue_unknown4;
		half Saturation_unknown4;
		half Value_unknown4;

		half MetallicBias_unknown4;
		half MetallicScale_unknown4;
		half SmoothnessBias_unknown4;
		half SmoothnessScale_unknown4;

        struct Input
        {
            float2 uv_MainTex;
			float3 worldPos;
        };

		


        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            
            float2 uv = IN.uv_MainTex;
			
			Sample_radius *= _TexY_TexelSize.x;
			

			float4 col = tet_sample(uv, 0, 0);
			o.Albedo = col;
			o.Alpha = 1;
			
			if (_NM != 0 && _Optimized != 1) {
				float4 n = tex2D(_TexNM, uv);
				float3 t = float3((n.r - 0.5) * 2, (n.g - 0.5) * 2, 0) * Normal_Map_Intensity;
				t.z = 1;
				o.Normal = normalize(t);
			}
			
			if (_MS != 1 ) {
                
				return;
			}
			
			int majority_label = tet_label(uv, 0, 0);

			if (_Optimized != 1) {
				static int majority[] = { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,
					0,0,0,0,0,0,0,0,0,0,0,0,0,0 };
				majority[majority_label]++;
				majority[tet_label(uv, 0, -Sample_radius)]++;
				majority[tet_label(uv, 0, +Sample_radius)]++;
				majority[tet_label(uv, -Sample_radius, 0)]++;
				majority[tet_label(uv, +Sample_radius, 0)]++;
				majority[tet_label(uv, +Sample_radius, -Sample_radius)]++;
				majority[tet_label(uv, -Sample_radius, +Sample_radius)]++;
				majority[tet_label(uv, -Sample_radius, -Sample_radius)]++;
				majority[tet_label(uv, +Sample_radius, +Sample_radius)]++;
				majority[tet_label(uv, +Sample_radius, +Sample_radius)]++;

				int majority_max = 0;

				for (int i = 1; i < 27; i++) {
					if (majority[i] > majority_max)
					{
						majority_label = i;
						majority_max = majority[i];
					}
				}
			}
			

			if (majority_label == 1) {
				//hat

				o.Albedo = hsv(col,Hue_hat,Saturation_hat,Value_hat) * Color_hat.rgb ;
				o.Metallic = saturate( MetallicScale_hat + MetallicBias_hat);
				o.Smoothness = saturate( SmoothnessScale_hat + SmoothnessBias_hat);
				clip(0.5 - Hide_hat);
				return;
			}

			if (majority_label == 2) {
				//hair
				
				o.Albedo = hsv(col,Hue_hair,Saturation_hair,Value_hair) * Color_hair.rgb ;
				o.Metallic = saturate( MetallicScale_hair + MetallicBias_hair);
				o.Smoothness = saturate( SmoothnessScale_hair + SmoothnessBias_hair);
				clip(0.5 - Hide_hair);
				return;
			}

			if (majority_label == 3) {
				//gloves

				o.Albedo = hsv(col, Hue_gloves, Saturation_gloves, Value_gloves) * Color_gloves.rgb;
				o.Metallic = saturate( MetallicScale_gloves + MetallicBias_gloves);
				o.Smoothness = saturate( SmoothnessScale_gloves + SmoothnessBias_gloves);
				clip(0.5 - Hide_gloves);
				return;
			}

			if (majority_label == 4) {
				//glasses

				o.Albedo = hsv(col, Hue_glasses, Saturation_glasses, Value_glasses) * Color_glasses.rgb;
				o.Metallic = saturate( MetallicScale_glasses + MetallicBias_glasses);
				o.Smoothness = saturate( SmoothnessScale_glasses + SmoothnessBias_glasses);
				clip(0.5 - Hide_glasses);
				return;
			}

			if (majority_label == 5) {
				// upper clothes

				o.Albedo = hsv(col,Hue_upper_clothes,Saturation_upper_clothes,Value_upper_clothes) * Color_upper_clothes.rgb ;
				o.Metallic = saturate( MetallicScale_upper_clothes + MetallicBias_upper_clothes);
				o.Smoothness = saturate( SmoothnessScale_upper_clothes + SmoothnessBias_upper_clothes);
				clip(0.5 - Hide_upper_clothes);
				return;
			}

			if (majority_label == 6) {
				//dress
				
				o.Albedo = hsv(col,Hue_dress,Saturation_dress,Value_dress) * Color_dress.rgb ;
				o.Metallic = saturate( MetallicScale_dress + MetallicBias_dress);
				o.Smoothness = saturate( SmoothnessScale_dress + SmoothnessBias_dress);
				clip(0.5 - Hide_dress);
				return;
			}

			if (majority_label == 7) {
				//coat
				
				o.Albedo = hsv(col,Hue_coat,Saturation_coat,Value_coat) * Color_coat.rgb ;
				o.Metallic = saturate( MetallicScale_coat + MetallicBias_coat);
				o.Smoothness = saturate( SmoothnessScale_coat + SmoothnessBias_coat);
				clip(0.5 - Hide_coat);
				return;
			}

			if (majority_label == 8) {
				// socks
				
				o.Albedo = hsv(col,Hue_socks,Saturation_socks,Value_socks) * Color_socks.rgb ;
				o.Metallic = saturate( MetallicScale_socks + MetallicBias_socks);
				o.Smoothness = saturate( SmoothnessScale_socks + SmoothnessBias_socks);
				clip(0.5 - Hide_socks);
				return;
			}

			if (majority_label == 9) {
				// pants

				o.Albedo = hsv(col,Hue_pants,Saturation_pants,Value_pants) * Color_pants.rgb ;
				o.Metallic = saturate( MetallicScale_pants + MetallicBias_pants);
				o.Smoothness = saturate( SmoothnessScale_pants + SmoothnessBias_pants);
				clip(0.5 - Hide_pants);
				return;
			}

			if (majority_label == 10) {
				// torso skin

				o.Albedo = hsv(col,Hue_torso_skin,Saturation_torso_skin,Value_torso_skin) * Color_torso_skin.rgb ;
				o.Metallic = saturate( MetallicScale_torso_skin + MetallicBias_torso_skin);
				o.Smoothness = saturate( SmoothnessScale_torso_skin + SmoothnessBias_torso_skin);
				clip(0.5 - Hide_torso_skin);
				return;
			}

			if (majority_label == 11) {
				//scarf
				
				o.Albedo = hsv(col,Hue_scarf,Saturation_scarf,Value_scarf) * Color_scarf.rgb ;
				o.Metallic = saturate( MetallicScale_scarf + MetallicBias_scarf);
				o.Smoothness = saturate( SmoothnessScale_scarf + SmoothnessBias_scarf);
				clip(0.5 - Hide_scarf);
				return;
			}

			if (majority_label == 12) {
				//skirt
				
				o.Albedo = hsv(col,Hue_skirt,Saturation_skirt,Value_skirt) * Color_skirt.rgb ;
				o.Metallic = saturate( MetallicScale_skirt + MetallicBias_skirt);
				o.Smoothness = saturate( SmoothnessScale_skirt + SmoothnessBias_skirt);
				clip(0.5 - Hide_skirt);
				return;
			}

			if (majority_label == 13) {
				//face
				
				o.Albedo = hsv(col,Hue_face,Saturation_face,Value_face) * Color_face.rgb ;
				o.Metallic = saturate( MetallicScale_face + MetallicBias_face);
				o.Smoothness = saturate( SmoothnessScale_face + SmoothnessBias_face);
				clip(0.5 - Hide_face);
				return;
			}

			if (majority_label == 14) {
				// left_arm
				
				o.Albedo = hsv(col,Hue_left_arm,Saturation_left_arm,Value_left_arm) * Color_left_arm.rgb ;
				o.Metallic = saturate( MetallicScale_left_arm + MetallicBias_left_arm);
				o.Smoothness = saturate( SmoothnessScale_left_arm + SmoothnessBias_left_arm);
				//clip(Color_left_arm.a - 0.05);
				clip(0.5 - Hide_left_arm);
				return;
			}

			if (majority_label == 15) {
				//right_arm
				
				o.Albedo = hsv(col,Hue_right_arm,Saturation_right_arm,Value_right_arm) * Color_right_arm.rgb ;
				o.Metallic = saturate( MetallicScale_right_arm + MetallicBias_right_arm);
				o.Smoothness = saturate( SmoothnessScale_right_arm + SmoothnessBias_right_arm);
				clip(0.5 - Hide_right_arm);
				return;
			}

			if (majority_label == 16) {
				// left_leg
				
				o.Albedo = hsv(col,Hue_left_leg,Saturation_left_leg,Value_left_leg) * Color_left_leg.rgb ;
				o.Metallic = saturate( MetallicScale_left_leg + MetallicBias_left_leg);
				o.Smoothness = saturate( SmoothnessScale_left_leg + SmoothnessBias_left_leg);
				clip(0.5 - Hide_left_leg);
				return;
			}

			if (majority_label == 17) {
				//right_leg
				
				o.Albedo = hsv(col,Hue_right_leg,Saturation_right_leg,Value_right_leg) * Color_right_leg.rgb ;
				o.Metallic = saturate( MetallicScale_right_leg + MetallicBias_right_leg);
				o.Smoothness = saturate( SmoothnessScale_right_leg + SmoothnessBias_right_leg);
				clip(0.5 - Hide_right_leg);
				return;
			}

			if (majority_label == 18) {
				//left_shoe
				
				o.Albedo = hsv(col,Hue_left_shoe,Saturation_left_shoe,Value_left_shoe) * Color_left_shoe.rgb ;
				o.Metallic = saturate( MetallicScale_left_shoe + MetallicBias_left_shoe);
				o.Smoothness = saturate( SmoothnessScale_left_shoe + SmoothnessBias_left_shoe);
				clip(0.5 - Hide_left_shoe);
				return;
			}

			if (majority_label == 19) {
				//right_shoe
				
				o.Albedo = hsv(col,Hue_right_shoe,Saturation_right_shoe,Value_right_shoe) * Color_right_shoe.rgb ;
				o.Metallic = saturate( MetallicScale_right_shoe + MetallicBias_right_shoe);
				o.Smoothness = saturate( SmoothnessScale_right_shoe + SmoothnessBias_right_shoe);
				clip(0.5 - Hide_right_shoe);
				return;
			}
			
			if (majority_label == 20) {
				// mouth
				
				o.Albedo = hsv(col,Hue_mouth,Saturation_mouth,Value_mouth) * Color_mouth.rgb ;
				o.Metallic = saturate( MetallicScale_mouth + MetallicBias_mouth);
				o.Smoothness = saturate( SmoothnessScale_mouth + SmoothnessBias_mouth);
				clip(0.5 - Hide_mouth);
				return;
			}
			if (majority_label == 21) {
				// left_eye
				
				o.Albedo = hsv(col,Hue_left_eye,Saturation_left_eye,Value_left_eye) * Color_left_eye.rgb ;
				o.Metallic = saturate( MetallicScale_left_eye + MetallicBias_left_eye);
				o.Smoothness = saturate( SmoothnessScale_left_eye + SmoothnessBias_left_eye);
				clip(0.5 - Hide_left_eye);
				return;
			}

			if (majority_label == 22) {
				// right_eye
				
				o.Albedo = hsv(col,Hue_right_eye,Saturation_right_eye,Value_right_eye) * Color_right_eye.rgb ;
				o.Metallic = saturate( MetallicScale_right_eye + MetallicBias_right_eye);
				o.Smoothness = saturate( SmoothnessScale_right_eye + SmoothnessBias_right_eye);
				clip(0.5 - Hide_right_eye);
				return;
			}

			
			if (majority_label == 23) {
				// unknown1
				
				o.Albedo = hsv(col,Hue_unknown1,Saturation_unknown1,Value_unknown1) * Color_unknown1.rgb ;
				o.Metallic = saturate( MetallicScale_unknown1 + MetallicBias_unknown1);
				o.Smoothness = saturate( SmoothnessScale_unknown1 + SmoothnessBias_unknown1);
				clip(0.5 - Hide_unknown1);
				return;
			}
			if (majority_label == 24) {
				// unknown2
				
				o.Albedo = hsv(col,Hue_unknown2,Saturation_unknown2,Value_unknown2) * Color_unknown2.rgb ;
				o.Metallic = saturate( MetallicScale_unknown2 + MetallicBias_unknown2);
				o.Smoothness = saturate( SmoothnessScale_unknown2 + SmoothnessBias_unknown2);
				clip(0.5 - Hide_unknown2);
				return;
			}
			if (majority_label == 25) {
				// unknown3
				
				o.Albedo = hsv(col,Hue_unknown3,Saturation_unknown3,Value_unknown3) * Color_unknown3.rgb ;
				o.Metallic = saturate( MetallicScale_unknown3 + MetallicBias_unknown3);
				o.Smoothness = saturate( SmoothnessScale_unknown3 + SmoothnessBias_unknown3);
				clip(0.5 - Hide_unknown3);
				return;
			}
			if (majority_label == 26) {
				// unknown4
				
				o.Albedo = hsv(col,Hue_unknown4,Saturation_unknown4,Value_unknown4) * Color_unknown4.rgb ;
				o.Metallic = saturate( MetallicScale_unknown4 + MetallicBias_unknown4);
				o.Smoothness = saturate( SmoothnessScale_unknown4 + SmoothnessBias_unknown4);
				clip(0.5 - Hide_unknown4);
				return;
			}

            
            
        }

        ENDCG
    } 
    FallBack "Diffuse"
    CustomEditor "TetaviShaderEditor"
}