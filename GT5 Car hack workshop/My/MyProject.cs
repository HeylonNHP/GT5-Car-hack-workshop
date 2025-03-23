using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.CompilerServices;

namespace GT5_Car_hack_workshop.My
{
    // Token: 0x02000004 RID: 4
    [HideModuleName]
    [GeneratedCode("MyTemplate", "10.0.0.0")]
    [StandardModule]
    internal sealed class MyProject
    {
        // Token: 0x04000001 RID: 1
        private static readonly ThreadSafeObjectProvider<MyForms> m_MyFormsObjectProvider = new();

        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000006 RID: 6 RVA: 0x00002948 File Offset: 0x00000D48
        [HelpKeyword("My.Forms")]
        internal static MyForms Forms => m_MyFormsObjectProvider.GetInstance;

        // Token: 0x02000005 RID: 5
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MyGroupCollection("System.Windows.Forms.Form", "Create__Instance__", "Dispose__Instance__", "My.MyProject.Forms")]
        internal sealed class MyForms
        {
            // Token: 0x04000009 RID: 9
            [ThreadStatic] private static Hashtable m_FormBeingCreated;

            // Token: 0x04000006 RID: 6
            public Customperformance m_Customperformance;

            // Token: 0x04000007 RID: 7
            public Form1 m_Form1;

            // Token: 0x04000008 RID: 8
            public TransmissionEditor MTransmissionEditor;

            // Token: 0x06000013 RID: 19 RVA: 0x00002BD8 File Offset: 0x00000FD8
            [DebuggerHidden]
            [EditorBrowsable(EditorBrowsableState.Never)]
            public MyForms()
            {
            }

            // Token: 0x17000006 RID: 6
            // (get) Token: 0x0600000B RID: 11 RVA: 0x000029C0 File Offset: 0x00000DC0
            // (set) Token: 0x0600000E RID: 14 RVA: 0x00002A2C File Offset: 0x00000E2C
            public Customperformance Customperformance
            {
                get => m_Customperformance = Create__Instance__(m_Customperformance);
                set
                {
                    if (value == m_Customperformance) return;
                    if (value is not null) throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_Customperformance);
                }
            }

            // Token: 0x17000007 RID: 7
            // (get) Token: 0x0600000C RID: 12 RVA: 0x000029E4 File Offset: 0x00000DE4
            // (set) Token: 0x0600000F RID: 15 RVA: 0x00002A54 File Offset: 0x00000E54
            public Form1 Form1
            {
                get => m_Form1 = Create__Instance__(m_Form1);
                set
                {
                    if (value == m_Form1) return;
                    if (value is not null) throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_Form1);
                }
            }

            // Token: 0x17000008 RID: 8
            // (get) Token: 0x0600000D RID: 13 RVA: 0x00002A08 File Offset: 0x00000E08
            // (set) Token: 0x06000010 RID: 16 RVA: 0x00002A7C File Offset: 0x00000E7C
            public TransmissionEditor TransmissionEditor
            {
                get => MTransmissionEditor = Create__Instance__(MTransmissionEditor);
                set
                {
                    if (value == MTransmissionEditor) return;
                    if (value is not null) throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref MTransmissionEditor);
                }
            }

            // Token: 0x06000011 RID: 17 RVA: 0x00002AA4 File Offset: 0x00000EA4
            [DebuggerHidden]
            private static T Create__Instance__<T>(T Instance) where T : Form, new()
            {
                if (Instance is null || Instance.IsDisposed)
                {
                    if (m_FormBeingCreated is not null)
                    {
                        if (m_FormBeingCreated.ContainsKey(typeof(T))) 
                            throw new InvalidOperationException(Utils.GetResourceString("WinForms_RecursiveFormCreate"));
                    }
                    else
                    {
                        m_FormBeingCreated = new Hashtable();
                    }

                    m_FormBeingCreated.Add(typeof(T), null);
                    try
                    {
                        return new T();
                    }
                    catch (TargetInvocationException ex) when (ex.InnerException is not null)
                    {
                        var betterMessage = Utils.GetResourceString("WinForms_SeeInnerException", ex.InnerException.Message);
                        throw new InvalidOperationException(betterMessage, ex.InnerException);
                    }
                    finally
                    {
                        m_FormBeingCreated.Remove(typeof(T));
                    }
                }

                return Instance;
            }

            // Token: 0x06000012 RID: 18 RVA: 0x00002BB0 File Offset: 0x00000FB0
            [DebuggerHidden]
            private void Dispose__Instance__<T>(ref T instance) where T : Form
            {
                instance.Dispose();
                instance = default;
            }

            // Token: 0x06000014 RID: 20 RVA: 0x00002BE0 File Offset: 0x00000FE0
            [EditorBrowsable(EditorBrowsableState.Never)]
            public override bool Equals(object o) => base.Equals(RuntimeHelpers.GetObjectValue(o));

            // Token: 0x06000015 RID: 21 RVA: 0x00002BFC File Offset: 0x00000FFC
            [EditorBrowsable(EditorBrowsableState.Never)]
            public override int GetHashCode() => base.GetHashCode();

            // Token: 0x06000016 RID: 22 RVA: 0x00002C10 File Offset: 0x00001010
            [EditorBrowsable(EditorBrowsableState.Never)]
            internal new Type GetType() => typeof(MyForms);

            // Token: 0x06000017 RID: 23 RVA: 0x00002C28 File Offset: 0x00001028
            [EditorBrowsable(EditorBrowsableState.Never)]
            public override string ToString() => base.ToString();
        }

        // Token: 0x02000006 RID: 6
        [EditorBrowsable(EditorBrowsableState.Never)]
        [ComVisible(false)]
        internal sealed class ThreadSafeObjectProvider<T> where T : new()
        {
            // Token: 0x0400000A RID: 10
            [ThreadStatic] [CompilerGenerated] private static T m_ThreadStaticValue;

            // Token: 0x06000020 RID: 32 RVA: 0x00002D00 File Offset: 0x00001100
            [EditorBrowsable(EditorBrowsableState.Never)]
            [DebuggerHidden]
            public ThreadSafeObjectProvider()
            {
            }

            // Token: 0x17000009 RID: 9
            // (get) Token: 0x0600001F RID: 31 RVA: 0x00002CD8 File Offset: 0x000010D8
            internal T GetInstance
            {
                [DebuggerHidden]
                get
                {
                    if (m_ThreadStaticValue is null) 
                        m_ThreadStaticValue = new T();
                    return m_ThreadStaticValue;
                }
            }
        }
    }
}