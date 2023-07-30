'use client';

import Divider from '@mui/material/Divider';
import Grid from '@mui/material/Unstable_Grid2';
import Container from '@mui/material/Container';
import Typography from '@mui/material/Typography';

import { paths } from 'src/routes/paths';
import Markdown from 'src/components/markdown';
import CustomBreadcrumbs from 'src/components/custom-breadcrumbs';
import { _tags, _mock, _categories, _travelPosts } from 'src/_mock';

import PostTags from '../../blog/common/post-tags';
// import TravelNewsletter from '../travel-newsletter';
import PostAuthor from '../../blog/common/post-author';
import PostSidebar from '../../blog/common/post-sidebar';
import TravelPostHero from '../../blog/travel/travel-post-hero';
import PostSocialsShare from '../../blog/common/post-socials-share';
import TravelLatestPosts from '../../blog/travel/travel-latest-posts';

import Collapse from '@mui/material/Collapse';

import Stack, { StackProps } from '@mui/material/Stack';
import Iconify from 'src/components/iconify/iconify';
import { Link, Switch } from '@mui/material';
import FormProvider, { RHFSwitch } from 'src/components/hook-form';
import { useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as Yup from 'yup';
import { useBoolean } from 'src/hooks/use-boolean';
import { blue } from '@mui/material/colors';
// ----------------------------------------------------------------------

export const FormSchema = Yup.object().shape({
  switch: Yup.boolean().oneOf([true], 'Switch is required'),
});

export const defaultValues = {
  switch: false,
};

export default function PhraseView() {
  const { title, description, author, tags, content } = _travelPosts[0];

  const methods = useForm({
    resolver: yupResolver(FormSchema),
    defaultValues,
  });

  const {
    reset,
    control,
    setValue,
    handleSubmit,
    formState: { isSubmitting },
  } = methods;

  const onSubmit = handleSubmit(async (data) => {
    await new Promise((resolve) => setTimeout(resolve, 3000));
    console.log('DATA', data);
    reset();
  });

  return (
    <>
      {/* <TravelPostHero post={_travelPosts[0]} /> */}

      <Container>
        <CustomBreadcrumbs
          sx={{ my: 3 }}
          links={[
            { name: 'Media', href: '/' },
            { name: 'Phrases', href: paths.travel.posts },
            { name: 'やっと試験終わったね。' },
          ]}
        />
      </Container>

      {/* <Divider sx={{ mb: { xs: 6, md: 10 } }} /> */}

      <Container>
        <Grid container spacing={{ md: 8 }}>
          <Grid xs={12} md={8}>
            <Stack sx={{ marginBottom: '20px' }}>
              <Block title="Settings">
                <FormProvider methods={methods} onSubmit={onSubmit}>
                  <RHFSwitch name="switch-japanese" label="Show Japanese" />
                  <RHFSwitch name="switch-english" label="Show English" />
                </FormProvider>
              </Block>
            </Stack>

            {/* Phrase */}
            <Stack
              spacing={2}
              alignItems={{ xs: 'center', md: 'flex-start' }}
              sx={{ marginBottom: '40px' }}
            >
              <Stack direction="row" alignItems="center" sx={{ typography: 'subtitle1' }}>
                {/* <Iconify icon="carbon:email" width={24} sx={{ mr: 1 }} />  */}
                Phrase
              </Stack>

              <Stack spacing={1}>
                <Stack
                  direction="row"
                  alignItems="center"
                  sx={{ typography: 'subtitle2', color: 'rgb(100, 100, 100)' }}
                >
                  {/* <Iconify icon="carbon:email" width={24} sx={{ mr: 1 }} />  */}
                  Japanese
                </Stack>

                <Typography paddingLeft={2} variant="body1">
                  やっと試験終わったね。
                </Typography>
              </Stack>

              <Stack spacing={1}>
                <Stack
                  direction="row"
                  alignItems="center"
                  sx={{ typography: 'subtitle2', color: 'rgb(100, 100, 100)' }}
                >
                  {/* <Iconify icon="carbon:email" width={24} sx={{ mr: 1 }} />  */}
                  English
                </Stack>

                <Typography variant="body1" paddingLeft={2}>
                  Finally finished the exam, huh?
                </Typography>
              </Stack>
            </Stack>

            {/* Audio */}
            <Stack
              spacing={1}
              alignItems={{ xs: 'center', md: 'flex-start' }}
              sx={{ marginBottom: '40px' }}
            >
              <Stack direction="row" alignItems="center" sx={{ typography: 'subtitle1' }}
                marginBottom={2}>
                {/* <Iconify icon="carbon:email" width={24} sx={{ mr: 1 }} />  */}
                Audio
              </Stack>

              <Stack spacing={1}>
                <audio className="" controls>
                  <source
                    src="../../../audio/2020年6月1日 Level 4 試験どうだった？.mp3"
                    type="audio/mpeg"
                  />
                  Your browser does not support the audio element.
                </audio>
              </Stack>
            </Stack>

            {/* Context */}
            <Stack
              spacing={1}
              alignItems={{ xs: 'center', md: 'flex-start' }}
              sx={{ marginBottom: '40px' }}
            >
              <Stack direction="row" alignItems="center" sx={{ typography: 'subtitle1' }}>
                {/* <Iconify icon="carbon:email" width={24} sx={{ mr: 1 }} />  */}
                Context
              </Stack>

              <Stack spacing={1}>
                <Typography variant="body1" paddingLeft={2}>
                  Someone asking you a question.
                </Typography>
              </Stack>
            </Stack>

            {/* Words */}
            <Stack
              spacing={1}
              alignItems={{ xs: 'center', md: 'flex-start' }}
              sx={{ marginBottom: '40px' }}
            >
              <Stack direction="row" alignItems="center" sx={{ typography: 'subtitle1' }}>
                {/* <Iconify icon="carbon:email" width={24} sx={{ mr: 1 }} />  */}
                Words
              </Stack>

              <Stack spacing={1} paddingLeft={2}>
                <Link href={paths.support} color="inherit">
                  試験 - exam, test, trial
                </Link>

                <Link href={paths.support} color="inherit">
                  試験する - to examine
                </Link>
              </Stack>
            </Stack>

            {/* Kanji */}
            <Stack
              spacing={1}
              alignItems={{ xs: 'center', md: 'flex-start' }}
              sx={{ marginBottom: '40px' }}
            >
              <Stack direction="row" alignItems="center" sx={{ typography: 'subtitle1' }}>
                {/* <Iconify icon="carbon:email" width={24} sx={{ mr: 1 }} />  */}
                Kanji
              </Stack>

              <Stack spacing={1} paddingLeft={2}>
                <Link href={paths.support} color="inherit">
                  試 - something
                </Link>

                <Link href={paths.support} color="inherit">
                  験 - also something
                </Link>
              </Stack>
            </Stack>

            {/* Keigo */}
            <Stack
              spacing={1}
              alignItems={{ xs: 'center', md: 'flex-start' }}
              sx={{ marginBottom: '40px' }}
            >
              <Stack direction="row" alignItems="center" sx={{ typography: 'subtitle1' }}>
                {/* <Iconify icon="carbon:email" width={24} sx={{ mr: 1 }} />  */}
                Keigo
              </Stack>

              <Stack spacing={1} paddingLeft={2}>
                <Link href={paths.support} color="inherit">
                  Informal
                </Link>
              </Stack>
            </Stack>

            {/* Dialect */}
            <Stack
              spacing={1}
              alignItems={{ xs: 'center', md: 'flex-start' }}
              sx={{ marginBottom: '40px' }}
            >
              <Stack direction="row" alignItems="center" sx={{ typography: 'subtitle1' }}>
                {/* <Iconify icon="carbon:email" width={24} sx={{ mr: 1 }} />  */}
                Dialect
              </Stack>

              <Stack spacing={1} paddingLeft={2}>
                <Link href={paths.support} color="inherit">
                  Standard, Kantou
                </Link>
              </Stack>
            </Stack>

            {/* Jargon */}
            <Stack
              spacing={1}
              alignItems={{ xs: 'center', md: 'flex-start' }}
              sx={{ marginBottom: '40px' }}
            >
              <Stack direction="row" alignItems="center" sx={{ typography: 'subtitle1' }}>
                {/* <Iconify icon="carbon:email" width={24} sx={{ mr: 1 }} />  */}
                Jargon
              </Stack>

              <Stack spacing={1} paddingLeft={2}>
                <Link href={paths.support} color="inherit">
                  Standard
                </Link>
              </Stack>
            </Stack>

            {/* Referenced By */}
            <Stack
              spacing={1}
              alignItems={{ xs: 'center', md: 'flex-start' }}
              sx={{ marginBottom: '40px' }}
            >
              <Stack direction="row" alignItems="center" sx={{ typography: 'subtitle1' }}>
                {/* <Iconify icon="carbon:email" width={24} sx={{ mr: 1 }} />  */}
                Referenced By
              </Stack>

              <Stack spacing={1} paddingLeft={2}>
                <Link href={paths.support} color="blue" fontWeight={200}>
                  Yuta Aoki - Vocabulary - 2020年6月1 日Level 4 試験どうだった
                </Link>
              </Stack>
            </Stack>

            {/* <Typography variant="h5" sx={{ mb: 5 }}>
              {description}
            </Typography> */}

            {/* <Markdown content={content} firstLetter /> */}

            <PostTags tags={tags} />

            <PostSocialsShare />

            <Divider sx={{ mt: 8 }} />

            <PostAuthor author={author} />
          </Grid>

          <Grid xs={12} md={4}>
            <PostSidebar
              popularTags={_tags}
              author={author}
              categories={_categories}
              recentPosts={{ list: _travelPosts.slice(-4) }}
              advertisement={{
                title: 'Advertisement',
                description: 'Duis leo. Donec orci lectus, aliquam ut, faucibus non',
                imageUrl: _mock.image.travel(9),
                path: '',
              }}
            />
          </Grid>
        </Grid>
      </Container>

      <TravelLatestPosts posts={_travelPosts.slice(0, 4)} />

      {/* <TravelNewsletter /> */}
    </>
  );
}

interface BlockProps extends StackProps {
  title: string;
  children: React.ReactNode;
}

function Block({ title, children, ...other }: BlockProps) {
  const contentOpen = useBoolean(false);

  return (
    <Stack alignItems="flex-start" sx={{ width: 1 }} {...other}>
      <Stack
        direction="row"
        alignItems="center"
        justifyContent="space-between"
        onClick={contentOpen.onToggle}
        sx={{ width: 1, cursor: 'pointer' }}
      >
        <Typography typography="subtitle2" color="rgb(100, 100, 100)">
          {title}
        </Typography>

        <Iconify
          icon={contentOpen.value ? 'carbon:subtract' : 'carbon:add'}
          sx={{ color: 'text.secondary' }}
        />
      </Stack>

      <Collapse unmountOnExit in={contentOpen.value} sx={{ px: 0.5 }}>
        {children}
      </Collapse>
    </Stack>
  );
}
