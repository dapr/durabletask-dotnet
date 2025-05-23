// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;

namespace Dapr.DurableTask.Client.Grpc.Tests;

public class DurableTaskClientBuilderExtensionsTests
{
    [Fact]
    public void UseGrpc_Sets()
    {
        ServiceCollection services = new();
        DefaultDurableTaskClientBuilder builder = new(null, services);
        Action act = () => builder.UseGrpc();
        act.Should().NotThrow();
        builder.BuildTarget.Should().Be(typeof(GrpcDurableTaskClient));

        IServiceProvider provider = services.BuildServiceProvider();
        GrpcDurableTaskClientOptions options = provider.GetOptions<GrpcDurableTaskClientOptions>();

        options.Address.Should().BeNull();
        options.Channel.Should().BeNull();
    }

    [Fact]
    public void UseGrpc_Address_Sets()
    {
        ServiceCollection services = new();
        DefaultDurableTaskClientBuilder builder = new(null, services);
        builder.UseGrpc("localhost:9001");

        IServiceProvider provider = services.BuildServiceProvider();
        GrpcDurableTaskClientOptions options = provider.GetOptions<GrpcDurableTaskClientOptions>();

        options.Address.Should().Be("localhost:9001");
        options.Channel.Should().BeNull();
    }

    [Fact]
    public void UseGrpc_Channel_Sets()
    {
        GrpcChannel c = GetChannel();
        ServiceCollection services = new();
        DefaultDurableTaskClientBuilder builder = new(null, services);
        builder.UseGrpc(c);

        IServiceProvider provider = services.BuildServiceProvider();
        GrpcDurableTaskClientOptions options = provider.GetOptions<GrpcDurableTaskClientOptions>();

        options.Channel.Should().Be(c);
        options.Address.Should().BeNull();
    }

    [Fact]
    public void UseGrpc_Callback_Sets()
    {
        GrpcChannel c = GetChannel();
        ServiceCollection services = new();
        DefaultDurableTaskClientBuilder builder = new(null, services);
        builder.UseGrpc(opt => opt.Channel = c);

        IServiceProvider provider = services.BuildServiceProvider();
        GrpcDurableTaskClientOptions options = provider.GetOptions<GrpcDurableTaskClientOptions>();

        options.Channel.Should().Be(c);
        options.Address.Should().BeNull();
    }

    [Fact]
    public void UseDefaultVersion_DefaultVersion_Sets()
    {
        ServiceCollection services = new();
        DefaultDurableTaskClientBuilder builder = new(null, services);
        builder.UseDefaultVersion("0.1")
            .UseGrpc();

        IServiceProvider provider = services.BuildServiceProvider();
        GrpcDurableTaskClientOptions options = provider.GetOptions<GrpcDurableTaskClientOptions>();

        options.DefaultVersion.Should().Be("0.1");
    }

    static GrpcChannel GetChannel() => GrpcChannel.ForAddress("http://localhost:9001");
}
