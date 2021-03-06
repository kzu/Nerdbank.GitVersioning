﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nerdbank.GitVersioning;
using Xunit;

public class VersionOptionsTests
{
    [Fact]
    public void FromVersion()
    {
        var vo = VersionOptions.FromVersion(new Version(1, 2), "-pre");
        Assert.Equal(new Version(1, 2), vo.Version.Version);
        Assert.Equal("-pre", vo.Version.Prerelease);
        Assert.Null(vo.AssemblyVersion);
        Assert.Equal(0, vo.BuildNumberOffset);
    }

    [Fact]
    public void Equality()
    {
        var vo1a = new VersionOptions
        {
            Version = new SemanticVersion("1.2"),
            AssemblyVersion = new VersionOptions.AssemblyVersionOptions(new Version("1.3")),
            BuildNumberOffset = 2,
        };
        var vo1b = new VersionOptions
        {
            Version = new SemanticVersion("1.2"),
            AssemblyVersion = new VersionOptions.AssemblyVersionOptions(new Version("1.3")),
            BuildNumberOffset = 2,
        };

        var vo2VaryAV = new VersionOptions
        {
            Version = new SemanticVersion("1.2"),
            AssemblyVersion = new VersionOptions.AssemblyVersionOptions(new Version("1.4")),
        };
        var vo2VaryV = new VersionOptions
        {
            Version = new SemanticVersion("1.4"),
            AssemblyVersion = new VersionOptions.AssemblyVersionOptions(new Version("1.3")),
        };
        var vo2VaryO = new VersionOptions
        {
            Version = new SemanticVersion("1.2"),
            AssemblyVersion = new VersionOptions.AssemblyVersionOptions(new Version("1.3")),
            BuildNumberOffset = 3,
        };

        Assert.Equal(vo1a, vo1b);
        Assert.NotEqual(vo2VaryAV, vo1a);
        Assert.NotEqual(vo2VaryV, vo1a);
        Assert.NotEqual(vo2VaryO, vo1a);
    }

    [Fact]
    public void AssemblyVersionOptions_Equality()
    {
        var avo1a = new VersionOptions.AssemblyVersionOptions { };
        var avo1b = new VersionOptions.AssemblyVersionOptions { };
        Assert.Equal(avo1a, avo1b);
        Assert.NotEqual(avo1a, null);

        var avo2a = new VersionOptions.AssemblyVersionOptions
        {
            Version = new Version("1.5"),
        };
        var avo2b = new VersionOptions.AssemblyVersionOptions
        {
            Version = new Version("1.5"),
        };
        var avo3 = new VersionOptions.AssemblyVersionOptions
        {
            Version = new Version("2.5"),
        };
        Assert.Equal(avo2a, avo2b);
        Assert.NotEqual(avo2a, avo1a);

        var avo4 = new VersionOptions.AssemblyVersionOptions
        {
            Precision = VersionOptions.VersionPrecision.Build,
        };
        var avo5 = new VersionOptions.AssemblyVersionOptions
        {
            Precision = VersionOptions.VersionPrecision.Minor,
        };
        Assert.NotEqual(avo4, avo5);
    }

    [Fact]
    public void CloudBuildOptions_Equality()
    {
        var cbo1a = new VersionOptions.CloudBuildOptions { };
        var cbo1b = new VersionOptions.CloudBuildOptions { };
        Assert.Equal(cbo1a, cbo1b);

        var cbo2a = new VersionOptions.CloudBuildOptions
        {
            SetVersionVariables = !cbo1a.SetVersionVariables,
        };
        Assert.NotEqual(cbo2a, cbo1a);

        var cbo3a = new VersionOptions.CloudBuildOptions
        {
            BuildNumber = new VersionOptions.CloudBuildNumberOptions { },
        };
        Assert.Equal(cbo3a, cbo1a); // Equal because we haven't changed defaults.

        var cbo4a = new VersionOptions.CloudBuildOptions
        {
            BuildNumber = new VersionOptions.CloudBuildNumberOptions
            {
                Enabled = !(new VersionOptions.CloudBuildNumberOptions().Enabled),
            },
        };
        Assert.NotEqual(cbo4a, cbo1a);
    }

    [Fact]
    public void CloudBuildNumberOptions_Equality()
    {
        var bno1a = new VersionOptions.CloudBuildNumberOptions { };
        var bno1b = new VersionOptions.CloudBuildNumberOptions { };
        Assert.Equal(bno1a, bno1b);

        var bno2a = new VersionOptions.CloudBuildNumberOptions
        {
            Enabled = !bno1a.Enabled,
        };
        Assert.NotEqual(bno1a, bno2a);

        var bno3a = new VersionOptions.CloudBuildNumberOptions
        {
            IncludeCommitId = new VersionOptions.CloudBuildNumberCommitIdOptions { },
        };
        Assert.Equal(bno1a, bno3a); // we haven't changed any defaults, even if it's non-null.

        var bno4a = new VersionOptions.CloudBuildNumberOptions
        {
            IncludeCommitId = new VersionOptions.CloudBuildNumberCommitIdOptions { When = VersionOptions.CloudBuildNumberCommitWhen.Never },
        };
        Assert.NotEqual(bno1a, bno4a);
    }

    [Fact]
    public void CloudBuildNumberCommitIdOptions_Equality()
    {
        var cio1a = new VersionOptions.CloudBuildNumberCommitIdOptions { };
        var cio1b = new VersionOptions.CloudBuildNumberCommitIdOptions { };
        Assert.Equal(cio1a, cio1b);

        var cio2a = new VersionOptions.CloudBuildNumberCommitIdOptions
        {
            When = (VersionOptions.CloudBuildNumberCommitWhen)((int)cio1a.When + 1),
        };
        Assert.NotEqual(cio1a, cio2a);

        var cio3a = new VersionOptions.CloudBuildNumberCommitIdOptions
        {
            Where = (VersionOptions.CloudBuildNumberCommitWhere)((int)cio1a.Where + 1),
        };
        Assert.NotEqual(cio1a, cio3a);
    }
}
