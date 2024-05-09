using System;
using System.Runtime.InteropServices;

public class TetaviCAPI
{
#if UNITY_ANDROID && !UNITY_EDITOR
public const string m_pluginName = "decoder_engine_api";
#else
	#if (UNITY_IOS || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX)
		#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
		public const string m_pluginName = "tetCodecMacLib";
		#else
		public const string m_pluginName = "__Internal";
		#endif
	#else
		public const string m_pluginName = "TV_DecoderEngine";
	#endif
#endif

	public enum ColorType
    {
        RGB,
        RGBA,
        CompressedDDS,
        YUV2,
        UserDefined,
        INVALID
    };

    public struct TextureInfo
    {
        public Int32 width,
            height,
            channels,
            stride;
        public ColorType color_type;
        public IntPtr pixels;
    };

	public struct AudioInfo
	{
		public UInt32 bytes_per_second,
			bits_per_sample,
			chanel_num,
			block_align,
			decoded_bytes_number, 
			samples_number;
	};

	public delegate void DebugCallback(string tag, string message);
	public static void DebugMethod(string tag, string message)
	{
    
    #if DEVELOPMENT_BUILD
		    UnityEngine.Debug.LogError($"TAG :: {tag}, message :: {message}");
    #else
            UnityEngine.Debug.Log($"TAG :: {tag}, message :: {message}");
    #endif
	}
        //#if !UNITY_IOS
	public static void register_debug_callback_default()
    {
    #if ENABLE_IL2CPP
            register_debug_callback(null);
            return;
    #endif
#if UNITY_IOS
#else

        register_debug_callback(DebugMethod);
    #endif
	}
    [DllImport(m_pluginName)]
    public static extern void register_debug_callback(DebugCallback callback);

    [DllImport(m_pluginName)]
	public static extern IntPtr create_stream_rendering();
	[DllImport(m_pluginName)]
	public static extern IntPtr create_audio_obj(IntPtr native_stream_ptr, [MarshalAs(UnmanagedType.LPStr)] string s);
	[DllImport(m_pluginName)]
	public static extern IntPtr get_render_func();
	[DllImport(m_pluginName)]
	public static extern int stream_get_id(IntPtr native_stream_ptr);
	[DllImport(m_pluginName)]
	public static extern void stream_apply_rendering_frame(IntPtr native_stream_ptr, IntPtr native_frame_ptr);
    [DllImport(m_pluginName)]
    public static extern void stream_set_textures_N12_targets(IntPtr native_stream_ptr, IntPtr texY, IntPtr texUV, int width, int height);
    [DllImport(m_pluginName)]
	public static extern void stream_set_textures_YUV420_targets(IntPtr native_stream_ptr, IntPtr texY, IntPtr texU, IntPtr texV, int width, int height);
	
    [DllImport(m_pluginName)]
    public static extern IntPtr create_stream_decoder_obj();
    [DllImport(m_pluginName)]
    public static extern IntPtr create_stream_decoder_obj_YUV();
    [DllImport(m_pluginName)]
	public static extern void stream_release(IntPtr native_stream_ptr);
	[DllImport(m_pluginName)]
	public static extern bool stream_has_audio(IntPtr native_stream_ptr);
	[DllImport(m_pluginName)]
	public static extern bool stream_init_model(IntPtr native_stream_ptr, [MarshalAs(UnmanagedType.LPStr)] string s);
	[DllImport(m_pluginName)]
	public static extern int stream_get_frame_count(IntPtr native_stream_ptr);
	[DllImport(m_pluginName)]
	public static extern int stream_get_fps(IntPtr native_stream_ptr);
	[DllImport(m_pluginName)]
	public static extern TextureInfo stream_get_video_width_height(IntPtr native_stream_ptr);
	[DllImport(m_pluginName)]
	public static extern int stream_frames_decoded_ahead(IntPtr native_stream_ptr);
	[DllImport(m_pluginName)]
	public static extern int stream_is_buffering(IntPtr native_stream_ptr);
	[DllImport(m_pluginName)]
	public static extern void stream_set_calculate_normals(IntPtr native_stream_ptr, bool calc);
    [DllImport(m_pluginName)]
    public static extern void stream_set_calculate_tangents(IntPtr native_stream_ptr, bool calc);
    [DllImport(m_pluginName)]
	public static extern IntPtr stream_get_frame_obj(IntPtr native_stream_ptr, int frame_id);
	[DllImport(m_pluginName)]
	public static extern IntPtr stream_set_progiling_debug_messages(bool state);
	[DllImport(m_pluginName)]
	public static extern int frame_get_id(IntPtr native_frame_ptr);
	[DllImport(m_pluginName)]
	public static extern int frame_get_vertex_count(IntPtr native_frame_ptr);
	[DllImport(m_pluginName)]
	public static extern int frame_get_uv_count(IntPtr native_frame_ptr);
    [DllImport(m_pluginName)]
    public static extern int frame_face_count(IntPtr native_frame_ptr);
    [DllImport(m_pluginName)]
    public static extern IntPtr frame_get_vertex_tangents(IntPtr frame);
    [DllImport(m_pluginName)]
    public static extern bool frame_copy_tangents(IntPtr native_frame_ptr, IntPtr native_bufferr_ptr);
    [DllImport(m_pluginName)]
    public static extern IntPtr frame_get_normal_map(IntPtr native_frame_ptr, ref int size, ref int width, ref int height);
    [DllImport(m_pluginName)]
    public static extern int frame_gop_id(IntPtr frame);
    [DllImport(m_pluginName)]
    public static extern IntPtr frame_get_texture_obj(IntPtr nativeFramePtr);
    [DllImport(m_pluginName, CallingConvention = CallingConvention.Cdecl)]
    public static extern TextureInfo texture_get_info(IntPtr nativeTexturePtr);
	[DllImport(m_pluginName)]
	public static extern int frame_is_key(IntPtr native_frame_ptr);
	[DllImport(m_pluginName)]
	public static extern void frame_copy_vertex_xyz(IntPtr native_frame_ptr, IntPtr native_bufferr_ptr);
    [DllImport(m_pluginName)]
    public static extern void frame_copy_normal_map(IntPtr native_frame_ptr, IntPtr native_bufferr_ptr, ref int width, ref int height);
    [DllImport(m_pluginName)]
	public static extern bool frame_copy_vertex_normals(IntPtr native_frame_ptr, IntPtr native_bufferr_ptr); // return false if not set calculate normals ahead so they were not computed
	[DllImport(m_pluginName)]
	public static extern void frame_copy_uv_data(IntPtr native_frame_ptr, bool reverse_v_sign, IntPtr native_bufferr_ptr);
	[DllImport(m_pluginName)]
	public static extern void frame_copy_face_indices(IntPtr native_frame_ptr, IntPtr native_bufferr_ptr);
	[DllImport(m_pluginName, CallingConvention = CallingConvention.Cdecl)]
	public static extern TextureInfo texture_copy_texture(IntPtr native_texture_ptr, IntPtr native_bufferr_ptr);
	[DllImport(m_pluginName, CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr /* char const */ get_profiling_info(IntPtr native_stream_ptr);
	[DllImport(m_pluginName, CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr /* char const */ get_mat_class_info(IntPtr native_stream_ptr);
	[DllImport(m_pluginName)]
	public static extern void audio_player_release(IntPtr native_audio_player_ptr);
	[DllImport(m_pluginName, CallingConvention = CallingConvention.Cdecl)]
	public static extern AudioInfo audio_player_get_info(IntPtr native_audio_player_ptr);
	[DllImport(m_pluginName)]
	public static extern int audio_player_get_audio_buffer_byte_size(IntPtr native_audio_player_ptr, int frame_idx);
	[DllImport(m_pluginName)]
	public static extern bool audio_player_copy_frame(IntPtr native_audio_player_ptr, int frame_idx, ref IntPtr dest_byte_arr);

}
