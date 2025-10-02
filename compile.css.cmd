
::del *.exe

::csc.exe /target:winexe /out:1kmpv1k.exe wait_async_mpv_1k-mod_time.cs toggle1k.cs
csc.exe /target:winexe /out:1kmpv1k.exe "wait_async_mpv_1k-mod_time (modified by chatgpt).cs" toggle1k.cs

::pause
