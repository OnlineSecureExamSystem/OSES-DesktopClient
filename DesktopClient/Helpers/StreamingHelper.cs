using Avalonia.Media.Imaging;
using Avalonia.Threading;
using DesktopClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace DesktopClient.Helpers
{
    public class StreamingHelper
    {
        //const string ffmpegLibFullPath = @"C:\Users\rd07g\Desktop\OSES\desktop-avalonia-client\DesktopClient\Libraries\ffmpeg\";
        //string path = @"C:\Users\rd07g\Desktop\OSES\desktop-avalonia-client\DesktopClient\Assets\Facebook.mp4";
        //public RTCPeerConnection pc;
        //public unsafe void Loop()
        //{
        //    AVDictionary* dict = null;

        //    ffmpeg.av_dict_set(&dict, "stimeout", $"{TimeSpan.FromSeconds(5).TotalMilliseconds * 1000}", 0);

        //    var ctx = ffmpeg.avformat_alloc_context();

        //    if (ffmpeg.avformat_open_input(&ctx, path, null, &dict) != 0)
        //    {
        //        ffmpeg.avformat_close_input(&ctx);
        //        throw new Exception("Cannot open input file");
        //    }

        //    ffmpeg.avformat_find_stream_info(ctx, null);

        //    var videoStreamIndex = -1;
        //    for (int i = 0; i < ctx->nb_streams; i++)
        //    {
        //        var stream = ctx->streams[i];
        //        if (stream->codecpar->codec_type == AVMediaType.AVMEDIA_TYPE_VIDEO)
        //        {
        //            videoStreamIndex = i;
        //        }
        //    }

        //    if (videoStreamIndex < 0)
        //    {
        //        ffmpeg.avformat_close_input(&ctx);
        //        throw new Exception("no video stream found");
        //    }

        //    ffmpeg.av_read_play(ctx);
        //    var pkt = ffmpeg.av_packet_alloc();


        //    Dispatcher.UIThread.InvokeAsync(() =>
        //    {
        //        ExceptionNotifier.NotifySuccess("Connected to stream");
        //    });



        //    while (true)
        //    {
        //        ffmpeg.av_init_packet(pkt);

        //        if (ffmpeg.av_read_frame(ctx, pkt) != 0)
        //        {
        //            ffmpeg.avformat_close_input(&ctx);
        //            ffmpeg.av_packet_free(&pkt);
        //            throw new Exception("Error reading stream");
        //        }

        //        if (pkt->stream_index != videoStreamIndex || pkt->pts < 0)
        //        {
        //            ffmpeg.av_packet_unref(pkt);
        //            continue;
        //        }
        //        var Out = pkt->pts;

        //        Dispatcher.UIThread.InvokeAsync(() =>
        //       {
        //           ExceptionNotifier.NotifySuccess(Out.ToString());
        //       });

        //        var mem = new Span<byte>(pkt->data, pkt->size);

        //        pc.SendVideo((uint)pkt->pts, mem.ToArray());

        //        ffmpeg.av_packet_unref(pkt);
        //    }
        //}
        private const int WEBSOCKET_PORT = 8081;
        private const string STUN_URL = "stun:stun.sipsorcery.com";
        private List<Bitmap> streamingCash = new List<Bitmap>();
        public static CameraHelper camera = null;
        private const string TESTE_FILENAME = @"C:\Users\rd07g\Desktop\OSES\desktop-avalonia-client\DesktopClient\Assets\Facebook.mp4";
        private const int TEST_PATTERN_FRAMES_PER_SECOND = 5; //30;
        public static SystemRequirmentsViewModel VM { get; set; }

        public StreamingHelper(SystemRequirmentsViewModel vm)
        {
            VM = vm;
        }

        //public async void Init()
        //{
        //    var webSocketServer = new WebSocketServer(IPAddress.Any, WEBSOCKET_PORT);
        //    // starting web socket server
        //    Dispatcher.UIThread.InvokeAsync(() =>
        //        {
        //            ExceptionNotifier.NotifyInfo("Starting web socket server...");
        //        });

        //    webSocketServer.AddWebSocketService<WebRTCWebSocketPeer>("/", (peer) => peer.CreatePeerConnection = () => CreatePeerConnection());
        //    webSocketServer.Start();

        //    Dispatcher.UIThread.InvokeAsync(() =>
        //    {
        //        ExceptionNotifier.NotifyInfo($"Waiting for web socket connections on {webSocketServer.Address}:{webSocketServer.Port}...");
        //    });
        //}

        public async void InitWebsocket()
        {
            var ws = new WebSocketServer("ws://127.0.0.1:8081");
            ws.AddWebSocketService<Echo>("/Echo");
            ws.Start();
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                ExceptionNotifier.NotifySuccess("Server started on: " + WEBSOCKET_PORT);
            });
        }

        public class Echo : WebSocketBehavior
        {
            private MemoryStream _buffer = new MemoryStream();


            protected override void OnMessage(MessageEventArgs e)
            {
                // clearing the buffer
                _buffer.SetLength(0);

                Bitmap temp = VM.CameraBitmap;
                temp.Save(_buffer);
                var byteArray = _buffer.ToArray();

                // sleeping for 100ms before sending so we match the frame rate of the video
                //Thread.Sleep(100);

                // converting a byte array to string
                var str = Convert.ToBase64String(byteArray);

                Send(str);
            }
        }

        public static byte[] ConvertImageBytes(byte[] imageBytes, ImageFormat imageFormat)
        {
            byte[] byteArray = new byte[0];
            FileStream stream = new FileStream("empty." + imageFormat, FileMode.Create);
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                stream.Write(byteArray, 0, byteArray.Length);
                byte[] buffer = new byte[16 * 1024];
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                byteArray = ms.ToArray();
                stream.Close();
                ms.Close();
            }
            return byteArray;
        }

        //async Task<RTCPeerConnection> CreatePeerConnection()
        //{

        //    pc = new RTCPeerConnection();

        //    var format = new VideoFormat(VideoCodecsEnum.H264, 102);

        //    var track = new MediaStreamTrack(format);

        //    pc.addTrack(track);

        //    pc.onsignalingstatechange += () =>
        //    {
        //        Dispatcher.UIThread.InvokeAsync(() =>
        //        {
        //            ExceptionNotifier.NotifyInfo("ice signaling state 1: " + pc.signalingState);
        //        });
        //    };

        //    pc.onicegatheringstatechange += (state) =>
        //    {
        //        Dispatcher.UIThread.InvokeAsync(() =>
        //        {
        //            ExceptionNotifier.NotifyInfo("ice gathering state 1: " + state);
        //        });
        //    };

        //    pc.oniceconnectionstatechange += (state) =>
        //    {
        //        Dispatcher.UIThread.InvokeAsync(() =>
        //        {
        //            ExceptionNotifier.NotifyInfo("ice connection state 1: " + state);
        //        });
        //    };

        //    pc.onicecandidateerror += (state, s) =>
        //    {
        //        Dispatcher.UIThread.InvokeAsync(() =>
        //        {
        //            ExceptionNotifier.NotifyError("ice connection state error: " + pc.iceConnectionState);
        //            ExceptionNotifier.NotifyError("ice candidate error state: " + state);
        //            ExceptionNotifier.NotifyError("ice candidate error string: " + s);
        //        });

        //    };

        //    pc.onconnectionstatechange += async (state) =>
        //    {
        //        Dispatcher.UIThread.InvokeAsync(() =>
        //        {
        //            ExceptionNotifier.NotifyInfo($"Peer connection state change to {state}.");
        //        });

        //        switch (state)
        //        {
        //            case RTCPeerConnectionState.connected:
        //                try
        //                {
        //                    Loop();
        //                }
        //                catch (Exception e)
        //                {
        //                    Dispatcher.UIThread.InvokeAsync(() =>
        //                    {
        //                        ExceptionNotifier.NotifyError(e.Message);
        //                    });
        //                }
        //                break;
        //            case RTCPeerConnectionState.failed:

        //                pc.Close("ice disconnection");

        //                break;
        //            case RTCPeerConnectionState.closed:

        //                //await mediaFileSource.CloseVideo();
        //                break;
        //        }
        //    };

        //    return pc;
        //}


    }
}
