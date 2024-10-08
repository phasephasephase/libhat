﻿using System.Diagnostics;
using System.Runtime.InteropServices;
using Hat;
using Hat.Extensions;

Console.WriteLine("libhat tests\n");

Console.WriteLine("scanning in memory buffer:");
var randomBytes = new byte[0x10000];
new Random().NextBytes(randomBytes);

var pattern = randomBytes.AsSpan().Slice(0x1000, 0x10).ToArray().AsPattern();
var scanner = new Scanner(Marshal.UnsafeAddrOfPinnedArrayElement(randomBytes, 0), (uint)randomBytes.Length);
var result = scanner.FindPattern(pattern);

if (result != null) Console.WriteLine($"found pattern at 0x{result.Address:X}");

Console.WriteLine("\nscanning in module:");

var modulePattern = new Pattern("48 89 5C 24 ? 48 89 6C 24 ? 48 89 74 24 ? 57 48 81 EC");

var module = Process.GetCurrentProcess().MainModule!;
var moduleScanner = new Scanner(module);
var moduleResult = moduleScanner.FindPattern(modulePattern);

if (moduleResult != null) Console.WriteLine($"found pattern at 0x{moduleResult.Address:X}");