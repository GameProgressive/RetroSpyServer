﻿using GameSpyLib.Common;
using GameSpyLib.Logging;
using GameSpyLib.Network;
using RetroSpyServer.Application;
using System;
using System.Collections.Generic;

namespace RetroSpyServer.Servers.PeerChat
{
        public class PeerChatClient : IDisposable
        {
            /// <summary>
            /// A unqie identifier for this connection
            /// </summary>
            public long ConnectionID;

            /// <summary>
            /// Indicates whether this object is disposed
            /// </summary>
            public bool Disposed { get; protected set; } = false;

            /// <summary>
            /// The clients socket network stream
            /// </summary>
            public TcpStream Stream { get; set; }

            /// <summary>
            /// Event fired when the connection is closed
            /// </summary>
            public static event PeerChatConnectionClosed OnDisconnect;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="client"></param>
            public PeerChatClient(TcpStream stream, long connectionId)
            {
                // Generate a unique name for this connection
                ConnectionID = connectionId;

                // Init a new client stream class
                Stream = stream;
                Stream.OnDisconnect += Dispose;
                //determine whether gamespy request is finished
                Stream.IsMessageFinished += Stream_IsMessageFinished;
                // Read client message, and parse it into key value pairs
                Stream.DataReceived += Stream_DataReceived;

            }

            /// <summary>
            /// Destructor
            /// </summary>
            ~PeerChatClient()
            {
                if (!Disposed)
                    Dispose();
            }

            public void Dispose()
            {
                // Only dispose once
                if (Disposed) return;
                Dispose(false);
            }

            /// <summary>
            /// Dispose method to be called by the server
            /// </summary>
            public void Dispose(bool DisposeEventArgs = false)
            {
                // Only dispose once
                if (Disposed) return;

                try
                {
                    Stream.OnDisconnect -= Dispose;
                    //determine whether gamespy request is finished
                    Stream.IsMessageFinished -= Stream_IsMessageFinished;
                    // Read client message, and parse it into key value pairs
                    Stream.DataReceived -= Stream_DataReceived;
                    // If connection is still alive, disconnect user
                    if (!Stream.SocketClosed)
                        Stream.Close(DisposeEventArgs);
                }
                catch { }


                // Call disconnect event
                OnDisconnect?.Invoke(this);

                Disposed = true;
            }
            private bool Stream_IsMessageFinished(string message)
            {
				return true;
            }
            /// <summary>
            /// This function is fired when data is received from a stream
            /// </summary>
            /// <param name="stream">The stream that sended the data</param>
            /// <param name="message">The message the stream sended</param>
            protected void Stream_DataReceived(string message)
            {
                LogWriter.Log.Write("[PEERCHAT] Recv " + message, LogLevel.Error);
            }
        }
    }

