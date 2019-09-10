﻿using System;
using Mod.Framework;

namespace OTAPI.Modules
{
    /// <summary>
    /// This module will handle any command line data such as what assemblies and query patterns
    /// </summary>
    [Module("Command line arguments", "death")]
    [AssemblyTarget("TerrariaServer, Version=1.3.5.3, Culture=neutral, PublicKeyToken=null")]
    [AssemblyTarget("Terraria, Version=1.3.5.3, Culture=neutral, PublicKeyToken=null")]
    public class CLI : RunnableModule
    {
        private ModFramework _framework;

        public CLI(ModFramework framework)
        {
            _framework = framework;
        }

        private HookOptions ParseFromPattern(ref string pattern)
        {
            var flags = HookOptions.None;
            var segments = pattern.Split('$');

            if (segments.Length == 2)
            {
                // valid
                pattern = segments[0];

                var character_flags = segments[1].ToCharArray();
                foreach (var flag in character_flags)
                {
                    switch (flag)
                    {
                        case 'b': // begin hook
                            flags |= HookOptions.Pre;
                            break;
                        case 'e': // end hook
                            flags |= HookOptions.Post;
                            break;
                        case 'r': // reference parameters
                            flags |= HookOptions.ReferenceParameters;
                            break;
                        case 'c': // begin hook can cancel
                            flags |= HookOptions.Cancellable;
                            break;
                        case 'a': // begin hook can alter non-void method return value
                            flags |= HookOptions.AlterResult;
                            break;
                        default:
                            throw new Exception($"Assembly Modification Pattern Flag is not valid: `{flag}`");
                    }
                }
            }
            else if (segments.Length > 2)
            {
                throw new Exception("Assembly Modification Patterns (AMP) only support flags defined at the end of the pattern");
            }

            return flags;
        }

        public override void Run()
        {
            //       List<string> inputs = new List<string>();
            //       List<string> modifications = new List<string>();
            //       OptionSet options = null;

            //       Console.WriteLine("Parsing command line arguments");

            //       var args = Environment.GetCommandLineArgs().Skip(1);
            //       if (args.Count() == 0)
            //       {
            //           args = new[]
            //           {
            //////@"-m=[TerrariaServer]Terraria.*,[TerrariaServer]ReLogic.*/rbe",
            ////@"-m=Terraria.Chest.Find*$berca",
            //@"-m=Terraria.Chest..ctor*$berca",
            //               @"-m=Terraria.Chest.*$berca",
            //               @"-m=Terraria.Main..c*$berca",
            //               @"-m=Terraria.Main.Initialize()*$berca",
            //               @"-m=Terraria.MessageBuffer.GetData(System.Int32,System.Int32,System.Int32&)$berca",
            //               @"-m=Terraria.NetMessage.*$berca",
            //               @"-m=ReLogic.OS.Platform..cctor*$berca",
            ////@"-m=Terraria.* && ReLogic.*$berca",
            //@"-m=Terraria.Net.*",
            //               @"-a=../../../TerrariaServer.exe",
            //               @"-a=../../../ReLogic.dll",
            //           };
            //       }

            //       options = new OptionSet();
            //       options.Add("m=|mod=|modification=", "Specify an assembly modification pattern (AMP)",
            //           op => modifications.Add(op));
            //       options.Add("a=|asm=|assembly=", "Specify an file glob for input assemblies",
            //           op => inputs.Add(op));

            //       options.Parse(args);

            //       if (modifications.Count == 0 || inputs.Count == 0)
            //       {
            //           options.WriteOptionDescriptions(Console.Out);
            //           return;
            //       }

            //       _framework.RegisterAssemblies(inputs.ToArray());

            //var t2 = new Query("NATUPNPLib.IStaticPortMapping", this.Assemblies).Run().Single().Instance as Mono.Cecil.TypeDefinition;
            //var t = new Query("Terraria.Chat.IChatProcessor", this.Assemblies).Run().Single().Instance as Mono.Cecil.TypeDefinition;
            //var tile = new Query("Terraria.Tile", this.Assemblies).Run().Single().Instance as Mono.Cecil.TypeDefinition;
            //foreach(var field in tile.Fields.Where(x => !x.HasConstant))
            //{
            //	field.FieldToProperty();
            //}
            //var i = new Mod.Framework.Emitters.InterfaceEmitter(tile);
            //var inte = i.Emit();
            //inte.Namespace = "OTAPI.Tile";
            ////tile.Module.Types.Add(inte);


            var modifications = new[]
            {
                "Terraria.Main..c*$berca"
            };
            foreach (var pattern in modifications)
            {
                Console.WriteLine($"\t-> Running query: {pattern}");
                var query_start = DateTime.Now;
                string query_pattern = pattern;
                var flags = ParseFromPattern(ref query_pattern);
                var res = new Query(query_pattern, this.Assemblies)
                    .Run()
                    .Hook(flags)
                ;
                Console.WriteLine($"\t\t-> Took: {(DateTime.Now - query_start).TotalMilliseconds}ms");
            }

            //var q = new Query("[TerrariaServer]Terraria.*&&[TerrariaServer]ReLogic.*", _modder.Assemblies);
            //var sub = q.Run();

            //var q2 = new Query("[TerrariaServer]Terraria.Chat.*", _modder.Assemblies);
            //var sub2 = q2.Run();

            //var q3 = new Query("Terraria.Chat.*", _modder.Assemblies);
            //var sub3 = q3.Run();
            //sub3.Hook();

            // hook only the Terraria.Main.Initialize method
            //new Query("Terraria.Main.Initialize()", _modder.CecilAssemblies)
            //	.Run()
            //	.Hook()
            //;
            //new Query("Terraria.Chest.AddShop(Terraria.Item)", _modder.CecilAssemblies)
            //	.Run()
            //	.Hook()
            //;
            //new Query("Terraria.Chest.Initialize()", _modder.CecilAssemblies)
            //	.Run()
            //	.Hook()
            //;
            //var asddd = new Query("Terraria.Chest.Find*", _modder.CecilAssemblies)
            //	.Run()
            //	.Select(x => x.Instance as MethodDefinition)
            //	.Where(x => x != null)
            //	.ToArray()
            //;

            //new Query("Terraria.Chest.FindEmptyChest(System.Int32,System.Int32,System.Int32,System.Int32,System.Int32)", _modder.CecilAssemblies)
            //	.Run()
            //	.Hook(HookFlags.Pre | HookFlags.Post | HookFlags.PreReferenceParameters)
            //;
            //var res = new Query("Terraria.Chest.Find*", _modder.CecilAssemblies)
            //	.Run()
            //	.Hook(HookFlags.Pre | HookFlags.Post | HookFlags.PreReferenceParameters)
            //;
            //// hook every method in Terraria.Main
            //new Query("Terraria.MessageBuffer.*&&Terraria.Netplay.*", _modder.CecilAssemblies)
            //	.Run()
            //	.Hook(HookFlags.Pre | HookFlags.Post | HookFlags.Cancellable)
            //;
        }
    }
}
